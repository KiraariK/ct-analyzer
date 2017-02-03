#include <vector>
#include <map>

#include "Node.h"

const unsigned char min_intencity = 0; // air on the ct image
const unsigned char max_intencity = 255; // body on the ct image

unsigned char* filtration(unsigned char* intencity_input, int images_number, int image_height, int image_width, int filtered_image_height,
	int filtered_image_width, int filter_width, int intencity_threshold);

void do_segmentation(unsigned char* input, int* region_numbers_array, int image_index, int map_height, int map_width,
	int** labelTD, int** labelLR, int** min, int** max, Node** nodes_map);

int merge_regions(unsigned char* input, int* region_numbers_array, int images_number, int image_height, int image_width);

void fill_result(int* result, int* region_numbers, int images_number, int image_height, int image_width,
	int filtered_image_height, int filtered_image_width, int filter_width);

int segmentation(unsigned char* intencity_input, int* regions_output, int images_number,
	int image_height, int image_width, int filter_width = 9, int intencity_threshold = 60)
{
	// filtration
	int filtered_image_height = image_height - filter_width + 1;
	int filtered_image_width = image_width - filter_width + 1;
	int filtered_image_size = filtered_image_height * filtered_image_width;

	unsigned char* filtered_images = filtration(intencity_input, images_number, image_height, image_width,
		filtered_image_height, filtered_image_width, filter_width, intencity_threshold);

	// segmentation
	int* region_numbers_array = new int[images_number * filtered_image_size];

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

	for (int k = 0; k < images_number; k++)
		do_segmentation(filtered_images, region_numbers_array, k, filtered_image_height, filtered_image_width,
			labelTD, labelLR, min, max, nodes_map);

	// merging of regions
	int regions_number = merge_regions(filtered_images, region_numbers_array, images_number,
		filtered_image_height, filtered_image_width);

	// fill result map
	fill_result(regions_output, region_numbers_array, images_number, image_height, image_width,
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

	delete[] filtered_images;
	delete[] region_numbers_array;

	return regions_number;
}

unsigned char* filtration(unsigned char* intencity_input, int images_number, int image_height, int image_width, int filtered_image_height,
	int filtered_image_width, int filter_width, int intencity_threshold)
{
	int original_image_size = image_height * image_width;
	int filtered_image_size = filtered_image_height * filtered_image_width;
	int filter_size = filter_width * filter_width;

	// creating 1D array for filtered images
	unsigned char* filtered_images = new unsigned char[images_number * filtered_image_size];

	// median filtering
	for (int k = 0; k < images_number; k++)
	{
		for (int i = 0; i < filtered_image_height; i++)
		{
			for (int j = 0; j < filtered_image_width; j++)
			{
				int sum = 0;
				for (int x = 0; x < filter_width; x++)
					for (int y = 0; y < filter_width; y++)
						sum += intencity_input[(k * original_image_size) + ((i + x) * image_width) + (j + y)];

				// thresholding
				filtered_images[(k * filtered_image_size) + (i * filtered_image_width) + j] =
					(sum / filter_size) > intencity_threshold ? max_intencity : min_intencity;
			}
		}
	}

	return filtered_images;
}

void do_segmentation(unsigned char* input, int* region_numbers_array, int image_index, int map_height, int map_width,
	int** labelTD, int** labelLR, int** min, int** max, Node** nodes_map)
{
	int map_size = map_height * map_width;

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
		if (input[(map_size * image_index) + (map_width * i)] == 0)
		{
			labelTD[i][0] = 0;
			labelLR[i][0] = 0;
		}

		// right border
		if (input[(map_size * image_index) + (map_width * i) + (map_width - 1)] == 0)
		{
			labelTD[i][map_width - 1] = 0;
			labelLR[i][map_width - 1] = 0;
		}
	}

	for (int j = 0; j < map_width; j++)
	{
		// top border
		if (input[(map_size * image_index) + j] == 0)
		{
			labelTD[0][j] = 0;
			labelLR[0][j] = 0;
		}

		// bottom border
		if (input[(map_size * image_index) + (map_width * (map_height - 1)) + j] == 0)
		{
			labelTD[map_height - 1][j] = 0;
			labelLR[map_height - 1][j] = 0;
		}
	}

	// from top to down
	for (int i = 0; i < map_width; i++)
	{
		for (int j = 1; j < map_height - 1; j++)
			if (input[(map_size * image_index) + (map_width * j) + i] == input[(map_size * image_index) + (map_width * (j - 1)) + i])
				labelTD[j][i] = labelTD[j - 1][i];
	}

	// from down to top
	bool isZeroNeighbour = false;
	for (int i = 0; i < map_width; i++)
	{
		unsigned char value = input[(map_size * image_index) + (map_width * (map_height - 1)) + i];
		if (labelTD[map_height - 1][i] == 0)
			isZeroNeighbour = true;

		for (int j = 1; j < map_height - 1; j++)
		{
			if (isZeroNeighbour == true)
			{
				if (value == input[(map_size * image_index) + (map_width * (map_height - j - 1)) + i])
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
			if (input[(map_size * image_index) + (map_width * i) + j] == input[(map_size * image_index) + (map_width * i) + (j - 1)])
				labelLR[i][j] = labelLR[i][j - 1];
	}

	// from right to left
	isZeroNeighbour = false;
	for (int i = 0; i < map_height; i++)
	{
		unsigned char value = input[(map_size * image_index) + (map_width * i) + (map_width - 1)];
		if (labelLR[i][map_width - 1] == 0)
			isZeroNeighbour = true;

		for (int j = 1; j < map_width - 1; j++)
		{
			if (isZeroNeighbour == true)
			{
				if (value == input[(map_size * image_index) + (map_width * i) + (map_width - j - 1)])
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
		if (input[(map_size * image_index) + (map_width * (map_height - 2)) + j] ==
			input[(map_size * image_index) + (map_width * (map_height - 1)) + j])
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
		if (input[(map_size * image_index) + (map_width * i) + (map_width - 2)] ==
			input[(map_size * image_index) + (map_width * i) + (map_width - 1)])
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
				region_numbers_array[(map_size * image_index) + (map_width * i) + j] = nodes_map[max[i][j]]->main_root->ID;
			}
			else
			{
				region_numbers_array[(map_size * image_index) + (map_width * i) + j] = min[i][j];
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

int merge_regions(unsigned char* input, int* region_numbers_array, int images_number, int image_height, int image_width)
{
	int image_size = image_height * image_width;

	// global normalization of region indexes from the first to the last
	int global_regions_counter = 0;
	std::vector<int> unique_region_numbers;
	for (int k = 0; k < images_number; k++)
	{
		for (int i = 0; i < image_height; i++)
		{
			for (int j = 0; j < image_width; j++)
			{
				bool is_in_list = false;
				for (unsigned int n = 0; n < unique_region_numbers.size(); n++)
				{
					if (region_numbers_array[(image_size * k) + (image_width * i) + j] == unique_region_numbers.at(n))
					{
						is_in_list = true;
						region_numbers_array[(image_size * k) + (image_width * i) + j] = global_regions_counter + n;
						break;
					}
				}

				if (!is_in_list)
				{
					unique_region_numbers.push_back(region_numbers_array[(image_size * k) + (image_width * i) + j]);
					region_numbers_array[(image_size * k) + (image_width * i) + j] = global_regions_counter + unique_region_numbers.size() - 1;
				}
			}
		}
		global_regions_counter += unique_region_numbers.size();
		unique_region_numbers.clear();
	}

	// create Nodes map as global regions
	Node** global_regions_map = new Node*[global_regions_counter];
	for (int i = 0; i < global_regions_counter; i++)
		global_regions_map[i] = new Node(i);

	// initialization of gloabl regions map
	for (int i = 0; i < global_regions_counter; i++)
	{
		global_regions_map[i]->main_root = global_regions_map[i];
		global_regions_map[i]->add_new_root(global_regions_map[i]);
	}

	// up-down merging of regions by comparison two neighbour slices
	for (int k = 1; k < images_number; k++)
	{
		for (int i = 0; i < image_height; i++)
		{
			for (int j = 0; j < image_width; j++)
			{
				// comparison the previous slice intencity and the current slice intencity
				if (input[(image_size * (k - 1)) + (image_width * i) + j] == input[(image_size * k) + (image_width * i) + j])
					global_regions_map[region_numbers_array[(image_size * k) + (image_width * i) + j]]->add_new_root(
						global_regions_map[region_numbers_array[(image_size * (k - 1)) + (image_width * i) + j]]);
			}
		}
	}

	// write globaly normalized region indexes to the segment_slices array and count the number of regions
	std::map<unsigned int, int> ids_map;
	unsigned int index_counter = 0;
	for (int k = 0; k < images_number; k++)
	{
		for (int i = 0; i < image_height; i++)
		{
			for (int j = 0; j < image_width; j++)
			{
				unsigned int current_id = global_regions_map[region_numbers_array[(image_size * k) + (image_width * i) + j]]->main_root->ID;

				if (ids_map.count(current_id) > 0) // index is already exist
					region_numbers_array[(image_size * k) + (image_width * i) + j] = ids_map[current_id];
				else
				{
					ids_map[current_id] = index_counter;
					index_counter++;
					region_numbers_array[(image_size * k) + (image_width * i) + j] = ids_map[current_id];
				}
			}
		}
	}
	int regions_counter = ids_map.size();
	ids_map.clear();

	// search the body region number
	//std::map<int, int> region_sizes;
	//for (int i = 0; i < image_height; i++)
	//{
	//	for (int j = 0; j < image_width; j++)
	//	{
	//		// filter for body regions
	//		if (input[(i * image_width) + j] < max_intencity) // analyze slice 0
	//			continue;

	//		int region_number = region_numbers_array[(i * image_width) + j]; // analyze slice 0
	//		if (region_sizes.count(region_number) > 0)
	//			region_sizes[region_number]++;
	//		else
	//			region_sizes[region_number] = 1;
	//	}
	//}

	//std::map<int, int>::const_iterator it;
	//int max_region_index = 0;
	//int max_region_number = 0;
	//for (it = region_sizes.begin(); it != region_sizes.end(); it++)
	//{
	//	int current_number = it->second;
	//	if (current_number > max_region_number)
	//	{
	//		max_region_index = it->first;
	//		max_region_number = current_number;
	//	}
	//}

	//region_sizes.clear();
	// end of searching the body region number

	// free memory
	for (int i = 0; i < global_regions_counter; i++)
		delete global_regions_map[i];
	delete[] global_regions_map;

	return regions_counter;
}

void fill_result(int* result, int* region_numbers, int images_number, int image_height, int image_width,
	int filtered_image_height, int filtered_image_width, int filter_width)
{
	int image_size = image_height * image_width;
	int filtered_image_size = filtered_image_height * filtered_image_width;
	int offset = filter_width / 2;

	for (int k = 0; k < images_number; k++)
	{
		for (int i = 0; i < image_height; i++)
		{
			for (int j = 0; j < image_width; j++)
			{
				// fill left and right bands as -1
				if (j < 0 + offset || j >= image_width - offset)
				{
					result[(k * image_size) + (i * image_width) + j] = -1;
					continue;
				}

				// fill top and bottom bands as -1
				if (i < 0 + offset || i >= image_height - offset)
				{
					result[(k * image_size) + (i * image_width) + j] = -1;
					continue;
				}

				result[(k * image_size) + (i * image_width) + j] =
					region_numbers[(k * filtered_image_size) + ((i - offset) * filtered_image_width) + (j - offset)];
			}
		}
	}
}