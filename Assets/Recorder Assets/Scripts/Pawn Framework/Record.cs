using UnityEngine;
using System.Collections;

namespace PawnFramework 
{

	public class Record 
	{
		private ArrayList m_Records;

		public Record()
		{
			m_Records = new ArrayList();
			AddRecordData(Vector3.zero, Vector3.zero, false, false);
		}

		public int Length
		{
			get { return m_Records.Count; }
		}
	
		public void AddRecordData(Vector3 directionVector, Vector3 aimDirection, bool jump, bool fire)
		{
			m_Records.Add(new RecordData(directionVector, aimDirection, jump, fire));
		}

		public RecordData GetRecordData(int index)
		{
			if(index < 0 || index >= m_Records.Count)
				return null;

			return m_Records[index] as RecordData;
		}

	}

	public class RecordData
	{
		public Vector3 DirectionVector;
		public Vector3 AimDirection;
		public bool Jump;
		public bool Fire;
		
		public RecordData ( Vector3 directionVector, Vector3 aimDirection, bool jump, bool fire )
		{
			this.DirectionVector = directionVector;
			this.AimDirection = aimDirection;
			this.Jump = jump;
			this.Fire = fire;
		}
	}
}