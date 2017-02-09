#pragma once

int slice_segmentation(unsigned char* intencity_input, int* regions_output,
	int image_height, int image_width, int filter_width = 9, int intencity_threshold = 60);