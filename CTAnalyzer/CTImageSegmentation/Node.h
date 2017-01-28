#pragma once

#include <vector>

class Node
{
public:
	unsigned int ID;
	Node* main_root;
	std::vector<Node*> children;
	std::vector<Node*> roots;

	Node(unsigned int id);
	~Node();

	Node* get_top_root();
	void add_new_root(Node* in_node);
};