using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {
	
    public GameObject BD_Enemy;
	
	public Vector3 StartVec;
	
	public int xVarying;
	
	public struct Wave {
		public int BDCount,
			SFCount,
			SSCount,
			BCCount,
			GDCount,
			MOCount,
			MSCount,
			SpawnDelay,
			StartTime;
		
		public Wave(int STime, int BD, int SF, int SS, int BC, int GD, int MO, int MS, int SDelay){
			this.BDCount = BD;
			this.SFCount = SF;
			this.SSCount = SS;
			this.BCCount = BC;
			this.GDCount = GD;
			this.MOCount = MO;
			this.MSCount = MS;
			this.StartTime = STime;
			this.SpawnDelay = SDelay
		}
	};
	
	public readonly Wave[] Waves = new[]
	{
				   //Time   BD  SF  SS  BC  GD  MO  MS SDelay
		new Wave(	1000,	5,	0,	0,	0,	0,	0,	0,	0),
		new Wave(	8000,	7,	3,	0,	0,	0,	0,	0,	100), //<== at 8 seconds, this wave spawns
		new Wave(	16000,	10,	5,	0,	0,	0,	0,	0,	200), //<== at 16 s
		new Wave(	24000,	12,	5,	3,	0,	0,	0,	0,	300),
		new Wave(	32000,	15,	5,	5,	0,	0,	0,	0,	400),
		//Null-Object, to sign the end of the array
		new Wave(	-1,		-1,	-1,	-1,	-1,	-1,	-1,	-1)	
	};
	
	//Elapsed Time in milliseconds
	public double Timer = 0;
	
	//Last Spawned wave
	public int LastSpawn = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	//Wave that is currently beeing spawned
	private Wave currSpawn;
	
	//Time at wich the last Enemy was Spawned
	private int lastSpawn;
	
	//X-Offset given to the last Enemy
	private int lastSpawnX;
	
	void SpawnWave(Wave AWave) {
		/*	for(int x = 0; x < AWave.BDCount; x++){
            	GameObject clone = Instantiate(BD_Enemy, StartVec + new Vector3(Random.value * xVarying - xVarying / 2, 0, 0), BD_Enemy.transform.rotation) as GameObject;
			}*/
		currSpawn = AWave;
		lastSpawn = 0;
		//Something Definelty out of bounds
		lastSpawnX = 0xFFFFF;
	}
	 
	void UpdateSpawns () {
			
	}
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime * 1000;
		if ((Timer > Waves[LastSpawn+1].StartTime) && (Waves[LastSpawn+1].StartTime != - 1) && (currSpawn.StartTime == - 1))  {
			SpawnWave(Waves[LastSpawn++]);
		}
		UpdateSpawns;
	}
}
