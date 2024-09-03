using UnityEngine;

namespace JigglePhysics
{
	public class PositionSignal
	{
		private struct Frame
		{
			public Vector3 position;
			public double time;
		}
		private Frame previousFrame;
		private Frame currentFrame;

		public PositionSignal(Vector3 startPosition, double time)
		{
			currentFrame = previousFrame = new Frame
			{
				position = startPosition,
				time = time,
			};
		}

		public void SetPosition(Vector3 position, double time)
		{
			previousFrame = currentFrame;
			currentFrame = new Frame
			{
				position = position,
				time = time,
			};
		}

		public void OffsetSignal(Vector3 offset)
		{
			previousFrame = new Frame
			{
				position = previousFrame.position + offset,
				time = previousFrame.time,
			};
			currentFrame = new Frame
			{
				position = currentFrame.position + offset,
				time = previousFrame.time,
			};
		}

		public void FlattenSignal(double time, Vector3 position, float fixedDeltaTime)
		{
			previousFrame = new Frame
			{
				position = position,
				time = time - JiggleRigBuilder.GetmaxCatchupTime(fixedDeltaTime) * 2f,
			};
			currentFrame = new Frame
			{
				position = position,
				time = time - JiggleRigBuilder.GetmaxCatchupTime(fixedDeltaTime),
			};
		}

		public Vector3 GetCurrent() => currentFrame.position;
		public Vector3 GetPrevious() => previousFrame.position;

		public Vector3 SamplePosition(double time)
		{
            double diff = currentFrame.time - previousFrame.time;
			if (diff == 0)
			{
				return previousFrame.position;
			}
			double t = ((double)(time) - (double)previousFrame.time) / (double)diff;
			return Vector3.Lerp(previousFrame.position, currentFrame.position, (float)t);
		}
	}
}
