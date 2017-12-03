<Query Kind="Statements" />

int input = 325489;

int[] offsets = new int[] { -1, 0, 1 };

int GetNeighbours(int[,] array, int center_x, int center_y)
{
	int sum = 0;
	foreach (var dx in offsets)
	{
		if (center_x + dx >= array.GetUpperBound(0) || center_x + dx < 0)
			continue;

		foreach (var dy in offsets)
		{
			if (center_y + dy >= array.GetUpperBound(1) || center_y + dy < 0)
				continue;

			sum += array[center_x + dx, center_y + dy];
		}
	}

	return sum;
}


var size = new[] { (int)Math.Floor(Math.Sqrt(input)), (int)Math.Floor(Math.Sqrt(input)) };
if (size[0] * size[1] < input)
{
	size[0] += 1;
	if (size[0] * size[1] < input)
		size[1] += 1;
}



var matrix = new int[size[0], size[1]];



int x = (int)Math.Floor(((size[0] - 1) / 2.0));
int y = (int)((size[1] - 1) / 2.0);
var direction = "right";
int steps_in_direction = 1;
int steps_in_direction_count = 0;
int steps = 0;

matrix[x, y] = 1;

var start = new[] { x, y };

foreach (var i in Enumerable.Range(2, input - 1))
{
	switch (direction)
	{
		case "right":
			x += 1;
			break;
		case "left":
			x -= 1;
			break;
		case "down":
			y += 1;
			break;
		case "up":
			y -= 1;
			break;
	}

	matrix[x, y] = GetNeighbours(matrix, x, y);

	if (matrix[x, y] > input)
	{
		matrix[x, y].Dump();
		break;
	}
	steps += 1;
	if (steps == steps_in_direction)
	{
		steps = 0;
		steps_in_direction_count++;
		if (steps_in_direction_count == 2)
		{
			steps_in_direction++;
			steps_in_direction_count = 0;
		}

		switch (direction)
		{
			case "right":
				direction = "down";
				break;
			case "left":
				direction = "up";
				break;
			case "down":
				direction = "left";
				break;
			case "up":
				direction = "right";
				break;

		}
	}


}
