<Query Kind="Program" />


public static string CURRENT_INSTRUCTION = "CI";
public static string BLOCKED = "BLOCKED";
public static string SENDCOUNT = "SENDCOUNT";

public static long GetValue(Dictionary<string, long> prog, string key)
{
	long res = 0;
	if (!long.TryParse(key, out res))
		if (!prog.TryGetValue(key, out res))
		{
			throw new NotImplementedException();
		}
	return res;
}

public static void ProcessInstruction(string[] instructions, Dictionary<string, long> prog, Queue<long> input, Queue<long> output)
{
	var parts = instructions[prog[CURRENT_INSTRUCTION]].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
	prog[CURRENT_INSTRUCTION]++;

	switch (parts[0])
	{
		case "snd":
			if (prog.ContainsKey(SENDCOUNT))
				prog[SENDCOUNT] += 1;

			output.Enqueue(GetValue(prog, parts[1]));
			break;

		case "rcv":
			if (input != null && input.Count > 0)
			{
				prog[parts[1]] = input.Dequeue();
				prog[BLOCKED] = 0;
			}
			else
			{
				prog[CURRENT_INSTRUCTION] -= 1;
				prog[BLOCKED] = 1;
			}
			break;


		case "add": prog[parts[1]] = GetValue(prog, parts[1]) + GetValue(prog, parts[2]); break;
		case "mul": prog[parts[1]] = GetValue(prog, parts[1]) * GetValue(prog, parts[2]); break;
		case "mod": prog[parts[1]] = GetValue(prog, parts[1]) % GetValue(prog, parts[2]); break;
		case "set": prog[parts[1]] = GetValue(prog, parts[2]); break;

		case "jgz":
			if (GetValue(prog, parts[1]) > 0)
				prog[CURRENT_INSTRUCTION] += GetValue(prog, parts[2]) - 1;

			break;



		default:
			throw new NotImplementedException();

	}
}

public static string A(string[] instructions)
{

	var p0 = new Dictionary<string, long>() { { BLOCKED, 0 }, { CURRENT_INSTRUCTION, 0 }, { "p", 0 } };

	Queue<long> melody = new Queue<long>();


	while (p0[BLOCKED] == 0)
		ProcessInstruction(instructions, p0, null, melody);

	return melody.Last().ToString();
}


public static string B(string[] instructions)
{

	var p0 = new Dictionary<string, long>() { { BLOCKED, 0 }, { CURRENT_INSTRUCTION, 0 }, { "p", 0 } };
	var p1 = new Dictionary<string, long>() { { BLOCKED, 0 }, { CURRENT_INSTRUCTION, 0 }, { "p", 1 }, { SENDCOUNT, 0 } };

	Queue<long> queue0 = new Queue<long>();
	Queue<long> queue1 = new Queue<long>();


	while (!(p0[BLOCKED] == 1 && p1[BLOCKED] == 1))
	{
		ProcessInstruction(instructions, p0, queue0, queue1);
		ProcessInstruction(instructions, p1, queue1, queue0);
	}

	return p1[SENDCOUNT].ToString();
}


void Main()
{
	var instructions = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
	//A(instructions).Dump();
	B(instructions).Dump();
}



private static string test = @"snd 1
snd 2
snd p
rcv a
rcv b
rcv c
rcv d";

private static string input = @"set i 31
set a 1
mul p 17
jgz p p
mul a 2
add i -1
jgz i -2
add a -1
set i 127
set p 618
mul p 8505
mod p a
mul p 129749
add p 12345
mod p a
set b p
mod b 10000
snd b
add i -1
jgz i -9
jgz a 3
rcv b
jgz b -1
set f 0
set i 126
rcv a
rcv b
set p a
mul p -1
add p b
jgz p 4
snd a
set a b
jgz 1 3
snd b
set f 1
add i -1
jgz i -11
snd a
jgz f -16
jgz a -19";