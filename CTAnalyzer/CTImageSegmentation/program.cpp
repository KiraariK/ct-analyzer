#include "segmentation.h"

// calling form external program
extern "C" _declspec (dllexport) int _cdecl CtSegmentation(unsigned char* intencity_input, int* regions_output,
	int images_number, int image_height, int image_width, int filter_width = 9, int intencity_threshold = 60)
{
	return segmentation(intencity_input, regions_output, images_number, image_height, image_width, filter_width, intencity_threshold);
}