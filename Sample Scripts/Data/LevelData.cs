using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class WaveData {
	public int Keyframe = 0;
	public List<EntityData> Entities = new List<EntityData>();
}

public class EntityData {
	public EnemyType Type = EnemyType.Basic;
	public int Count = 0;

}