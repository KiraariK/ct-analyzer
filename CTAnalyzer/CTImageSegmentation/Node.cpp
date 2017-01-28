#include "Node.h"

Node::Node(unsigned int id) : ID(id)
{
	main_root = NULL;
}

Node::~Node()
{}

Node* Node::get_top_root()
{
	if (main_root == this)
		return main_root;
	else
		return main_root->get_top_root();
}

void Node::add_new_root(Node* in_node)
{
	Node* in_node_top_root = in_node->get_top_root();
	bool check = false;

	for (unsigned int i = 0; i < roots.size(); i++)
	{
		if (roots.at(i) == in_node_top_root)
		{
			check = true;
			break;
		}
	}

	if (main_root != NULL)
	{
		if (main_root->ID < in_node_top_root->ID)
		{
			Node* top_node = in_node_top_root->get_top_root();
			top_node->add_new_root(main_root);
			check = true;
		}
	}

	if (check == false)
	{
		roots.push_back(in_node_top_root);

		if (in_node_top_root != this)
			in_node_top_root->children.push_back(this);

		if (roots.size() > 1)
		{
			if (main_root->ID > in_node_top_root->ID)
				main_root = in_node_top_root;
		}
		else
			main_root = in_node_top_root;

		for (unsigned int k = 0; k < children.size(); k++)
		{
			Node* child = children.at(k);
			if (child->main_root != in_node_top_root)
				child->add_new_root(in_node_top_root);
		}

		for (unsigned int k = 0; k < roots.size(); k++)
		{
			Node* root = roots.at(k);
			if (root != in_node_top_root)
			{
				if (root->main_root != in_node_top_root)
				{
					Node* top_node = root->get_top_root();
					top_node->add_new_root(in_node_top_root);
				}
			}
		}
	}
}