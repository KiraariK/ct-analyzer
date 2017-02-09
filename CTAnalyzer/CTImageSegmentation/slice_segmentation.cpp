#include <map>

#include "slice_segmentation.h"
#include "Node.h"

const unsigned char min_intencity = 0; // air on the ct image
const unsigned char max_intencity = 255; // body on the ct image

unsigned char* filtration(unsigned char* intencity_input, int image_height, int image_width, int filtered_image_height,
	int filtered_image_width, int filter_width, int intencity_threshold);

void do_segmentation(unsigned char* input, int* region_numbers_array, int map_height, int map_width,
	int** labelTD, int** labelLR, int** min, int** max, Node** nodes_map);

int regions_normalization(int* region_numbers_array, int image_height, int image_width);

void fill_result(int* result, int* region_numbers, int image_height, int image_width,
	int filtered_image_height, int filtered_image_width, int filter_width);

int slice_segmentation(unsigned char* intencity_input, int* regions_output,
	int image_height, int image_width, int filter_width, int intencity_threshold)
{
	// filtration
	int filtered_image_height = image_height - filter_width + 1;
	int filtered_image_width = image_width - filter_width + 1;
	int filtered_image_size = filtered_image_height * filtered_image_width;

	unsigned char* filtered_image = filtration(intencity_input, image_height, image_width,
		filtered_image_height, filtered_image_width, filter_width, intencity_threshold);

	// segmentation
	int* region_numbers_array = new int[filtered_image_size];

	// create nodes map
	Node** nodes_map = new Node*[filtered_image_size];
	for (int i = 0; i < filtered_image_size; i++)
		nodes_map[i] = new Node(i);

	// create TopDown, LeftRight maps, min and max maps
	int** labelTD = new int*[filtered_image_height];
	int** labelLR = new int*[filtered_image_height];
	int** min = new int*[filtered_image_height];
	int** max = new int*[filtered_image_height];
	for (int i = 0; i < filtered_image_height; i++)
	{
		labelTD[i] = new int[filtered_image_width];
		labelLR[i] = new int[filtered_image_width];
		min[i] = new int[filtered_image_width];
		max[i] = new int[filtered_image_width];
	}

	do_segmentation(filtered_image, region_numbers_array, filtered_image_height, filtered_image_width,
		labelTD, labelLR, min, max, nodes_map);

	// region numbers normalization
	int regions_number = regions_normalization(region_numbers_array, filtered_image_height, filtered_image_width);

	// fill result map
	fill_result(regions_output, region_numbers_array, image_height, image_width,
		filtered_image_height, filtered_image_width, filter_width);

	// free memory
	for (int i = 0; i < filtered_image_size; i++)
		delete nodes_map[i];
	delete[] nodes_map;

	for (unsigned short i = 0; i < filtered_image_height; i++)
	{
		delete[] labelTD[i];
		delete[] labelLR[i];
		delete[] min[i];
		delete[] max[i];
	}
	delete[] labelTD;
	delete[] labelLR;
	delete[] min;
	delete[] max;

	delete[] filtered_image;
	delete[] region_numbers_array;

	return regions_number;
}

unsigned char* filtration(unsigned char* intencity_input, int image_height, int image_width, int filtered_image_height,
	int filtered_image_width, int filter_width, int intencity_threshold)
{
	int filtered_image_size = filtered_image_height * filtered_image_width;
	int filter_size = filter_width * filter_width;

	// creating 1D array for filtered image
	unsigned char* filtered_image = new unsigned char[filtered_image_size];

	// median filtering
	for (int i = 0; i < filtered_image_height; i++)
	{
		for (int j = 0; j < filtered_image_width; j++)
		{
			int sum = 0;
			for (int x = 0; x < filter_width; x++)
				for (int y = 0; y < filter_width; y++)
					sum += intencity_input[((i + x) * image_width) + (j + y)];

			// thresholding
			filtered_image[(i * filtered_image_width) + j] =
				(sum / filter_size) > intencity_threshold ? max_intencity : min_intencity;
		}
	}

	return filtered_image;
}

void do_segmentation(unsigned char* input, int* region_numbers_array, int map_height, int map_width,
	int** labelTD, int** labelLR, int** min, int** max, Node** nodes_map)
{
	// fill labels maps
	for (int i = 0; i < map_height; i++)
	{
		for (int j = 0; j < map_width; j++)
		{
			labelTD[i][j] = i * map_width + j;
			labelLR[i][j] = i * map_width + j;
		}
	}

	// fill borders by zeros
	for (int i = 0; i < map_height; i++)
	{
		// left border
		if (input[(map_width * i)] == 0)
		{
			labelTD[i][0] = 0;
			labelLR[i][0] = 0;
		}

		// right border
		if (input[(map_width * i) + (map_width - 1)] == 0)
		{
			labelTD[i][map_width - 1] = 0;
			labelLR[i][map_width - 1] = 0;
		}
	}

	for (int j = 0; j < map_width; j++)
	{
		// top border
		if (input[j] == 0)
		{
			labelTD[0][j] = 0;
			labelLR[0][j] = 0;
		}

		// bottom border
		if (input[(map_width * (map_height - 1)) + j] == 0)
		{
			labelTD[map_height - 1][j] = 0;
			labelLR[map_height - 1][j] = 0;
		}
	}

	// from top to down
	for (int i = 0; i < map_width; i++)
	{
		for (int j = 1; j < map_height - 1; j++)
			if (input[(map_width * j) + i] == input[(map_width * (j - 1)) + i])
				labelTD[j][i] = labelTD[j - 1][i];
	}

	// from down to top
	bool isZeroNeighbour = false;
	for (int i = 0; i < map_width; i++)
	{
		unsigned char value = input[(map_width * (map_height - 1)) + i];
		if (labelTD[map_height - 1][i] == 0)
			isZeroNeighbour = true;

		for (int j = 1; j < map_height - 1; j++)
		{
			if (isZeroNeighbour == true)
			{
				if (value == input[(map_width * (map_height - j - 1)) + i])
					labelTD[map_height - j - 1][i] = 0;
				else
				{
					isZeroNeighbour = false;
					break;
				}
			}
		}
	}

	// from left to right
	for (int i = 0; i < map_height; i++)
	{
		for (int j = 1; j < map_width - 1; j++)
			if (input[(map_width * i) + j] == input[(map_width * i) + (j - 1)])
				labelLR[i][j] = labelLR[i][j - 1];
	}

	// from right to left
	isZeroNeighbour = false;
	for (int i = 0; i < map_height; i++)
	{
		unsigned char value = input[(map_width * i) + (map_width - 1)];
		if (labelLR[i][map_width - 1] == 0)
			isZeroNeighbour = true;

		for (int j = 1; j < map_width - 1; j++)
		{
			if (isZeroNeighbour == true)
			{
				if (value == input[(map_width * i) + (map_width - j - 1)])
					labelLR[i][map_width - j - 1] = 0;
				else
				{
					isZeroNeighbour = false;
					break;
				}
			}
		}
	}

	// min and max pams initialization
	for (int i = 0; i < map_height; i++)
	{
		for (int j = 0; j < map_width; j++)
		{
			if (labelTD[i][j] > labelLR[i][j])
			{
				max[i][j] = labelTD[i][j];
				min[i][j] = labelLR[i][j];
			}
			else
			{
				min[i][j] = labelTD[i][j];
				max[i][j] = labelLR[i][j];
			}
		}
	}

	// initialization of nodes_map
	for (int i = 0; i < map_height; i++)
	{
		for (int j = 0; j < map_width; j++)
		{
			if (min[i][j] == 0)
			{
				nodes_map[i * map_width + j]->main_root = nodes_map[0];
				nodes_map[i * map_width + j]->add_new_root(nodes_map[0]);
			}
			else
			{
				if (min[i][j] == max[i][j])
				{
					nodes_map[i * map_width + j]->main_root = nodes_map[i * map_width + j];
					nodes_map[i * map_width + j]->add_new_root(nodes_map[i * map_width + j]);
				}
			}
		}
	}

	// regions growth
	for (int i = 0; i < map_height; i++)
		for (int j = 0; j < map_width; j++)
			nodes_map[max[i][j]]->add_new_root(nodes_map[min[i][j]]);

	// postprocessing
	// for bottom border
	for (int j = 0; j < map_width; j++)
	{
		if (input[(map_width * (map_height - 2)) + j] ==
			input[(map_width * (map_height - 1)) + j])
		{
			if (nodes_map[max[map_height - 2][j]]->main_root != NULL)
			{
				if (nodes_map[max[map_height - 1][j]]->main_root != NULL)
					nodes_map[max[map_height - 1][j]]->main_root->ID = nodes_map[max[map_height - 2][j]]->main_root->ID;
				else
					min[map_height - 1][j] = nodes_map[max[map_height - 2][j]]->main_root->ID;
			}
			else
			{
				if (nodes_map[max[map_height - 1][j]]->main_root != NULL)
					nodes_map[max[map_height - 1][j]]->main_root->ID = min[map_height - 2][j];
				else
					min[map_height - 1][j] = min[map_height - 2][j];
			}
		}
	}
	// for right border
	for (int i = 0; i < map_height; i++)
	{
		if (input[(map_width * i) + (map_width - 2)] ==
			input[(map_width * i) + (map_width - 1)])
		{
			if (nodes_map[max[i][map_width - 2]]->main_root != NULL)
			{
				if (nodes_map[max[i][map_width - 1]]->main_root != NULL)
					nodes_map[max[i][map_width - 1]]->main_root->ID = nodes_map[max[i][map_width - 2]]->main_root->ID;
				else
					min[i][map_width - 1] = nodes_map[max[i][map_width - 2]]->main_root->ID;
			}
			else
			{
				if (nodes_map[max[i][map_width - 1]]->main_root != NULL)
					nodes_map[max[i][map_width - 1]]->main_root->ID = min[i][map_width - 2];
				else
					min[i][map_width - 1] = min[i][map_width - 2];
			}
		}
	}

	// fill result map
	for (int i = 0; i < map_height; i++)
	{
		for (int j = 0; j < map_width; j++)
		{
			if (nodes_map[max[i][j]]->main_root != NULL)
			{
				region_numbers_array[(map_width * i) + j] = nodes_map[max[i][j]]->main_root->ID;
			}
			else
			{
				region_numbers_array[(map_width * i) + j] = min[i][j];
			}
		}
	}

	// fill nodes_map by default parameters (it is cheeper than create\delete objects for each function call)
	for (int i = 0; i < map_height * map_width; i++)
	{
		nodes_map[i]->main_root = NULL;
		nodes_map[i]->children.clear();
		nodes_map[i]->roots.clear();
	}
}

int regions_normalization(int* region_numbers_array, int image_height, int image_width)
{
	// normalize region numbers by sequence 0, 1, ... and count number of regions
	std::map<unsigned int, int> ids_map;
	int index_counter = 0;
	for (int i = 0; i < image_height; i++)
	{
		for (int j = 0; j < image_width; j++)
		{
			unsigned int current_id = region_numbers_array[(image_width * i) + j];

			if (ids_map.count(current_id) > 0) // index is already exist
				region_numbers_array[(image_width * i) + j] = ids_map[current_id];
			else
			{
				ids_map[current_id] = index_counter;
				index_counter++;
				region_numbers_array[(image_width * i) + j] = ids_map[current_id];
			}
		}
	}
	int regions_counter = ids_map.size();
	ids_map.clear();

	return regions_counter;
}

void fill_result(int* result, int* region_numbers, int image_height, int image_width,
	int filtered_image_height, int filtered_image_width, int filter_width)
{
	int offset = filter_width / 2;

	for (int i = 0; i < image_height; i++)
	{
		for (int j = 0; j < image_width; j++)
		{
			// fill left and right bands as -1
			if (j < 0 + offset || j >= image_width - offset)
			{
				result[(i * image_width) + j] = -1;
				continue;
			}

			// fill top and bottom bands as -1
			if (i < 0 + offset || i >= image_height - offset)
			{
				result[(i * image_width) + j] = -1;
				continue;
			}

			result[(i * image_width) + j] = region_numbers[((i - offset) * filtered_image_width) + (j - offset)];
		}
	}
}