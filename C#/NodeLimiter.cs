using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class NodeLimiter : Node
{

	public static Dictionary<string, LimitQueue> queues = new Dictionary<string, LimitQueue>();
	[Export]
	string queueName;
	[Export]
	int queueLimit;
	



	public override void _Ready()
	{
		// check if queue exists
		if(queues.ContainsKey(queueName))
		{
			// add game object
			queues[queueName].AddNode(this as Node);
		}
		else
		{
			// create queue
			queues.Add(queueName, new LimitQueue(){limit = queueLimit});
			// add game object
			queues[queueName].AddNode(this as Node);
		}
		
	}
}



public class LimitQueue
{

	public Queue<Node> queue = new Queue<Node>();
	public int limit;



	public void AddNode(Node nodeToAdd)
	{
		// check if queue is full
		if(queue.Count >= limit)
		{	
			// remove oldest game object
			var nodeToDestroy = queue.Dequeue();
			nodeToDestroy.Owner.QueueFree();
		}

		// add new game object
		queue.Enqueue(nodeToAdd);
	}
}
