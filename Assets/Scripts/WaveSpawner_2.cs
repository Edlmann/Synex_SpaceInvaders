using UnityEngine;
using System.Collections;

public class WaveSpawner_2 : MonoBehaviour {
	
    public GameObject BD_Enemy;
    public GameObject SF_Enemy;
    public GameObject SS_Enemy;
    public GameObject BC_Enemy;
    public GameObject GD_Enemy;
    public GameObject MO_Enemy;
    public GameObject MS_Enemy;
	
	public Vector3 StartVec;
	
	public int xVarying;
	
	public float crap1;
	
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
			this.SpawnDelay = SDelay;
		}
	};
	
	public readonly Wave[] Waves = new[]
	{
				   //Time   BD  SF  SS  BC  GD  MO  MS SDelay
		new Wave(	1000,	5,	0,	0,	0,	0,	0,	0,	1000),
		new Wave(	4000,	7,	3,	0,	0,	0,	0,	0,	1000), //<== at 8 seconds, this wave spawns
		new Wave(	8000,	10,	5,	0,	0,	0,	0,	0,	1000), //<== at 16 s
		new Wave(	12000,	12,	5,	3,	0,	0,	0,	0,	1000),
		new Wave(	16000,	15,	5,	5,	0,	0,	0,	0,	1000),
		//Null-Object, to sign the end of the array
		new Wave(	-1,		-1,	-1,	-1,	-1,	-1,	-1,	-1, -1)	
	};
	
	//Elapsed Time in milliseconds
	public double Timer = 0;
	
	//Last Spawned wave
	public int LastSpawn = -1;
	
	//All Data needed about the last Enemy-Spawn
	private struct LastEnemySpawn {
		public double TimeStamp;
		public float XPos;
		public float XSize;
	}
	
	//Time at wich the last Enemy was Spawned
	private LastEnemySpawn lastEnemySpawn;
	
	//Wave that is currently beeing spawned
	private Wave currSpawn = new Wave(0,0,0,0,0,0,0,0,0);

	// Use this for initialization
	void Start () {
		lastEnemySpawn.TimeStamp = - 1;
		currSpawn.StartTime = - 1;
	}
	
	
	/*=========================================================
	
		Sets the given Wave as Wave to Spawn
	
	=========================================================*/	
	void SpawnWave(Wave AWave) {
		GameObject toSpawn = BD_Enemy;
		GameObject clone = Instantiate(toSpawn, StartVec, toSpawn.transform.rotation) as GameObject;
		//Copy all Values to currSpawn
		currSpawn.BDCount = AWave.BDCount;
		currSpawn.SFCount = AWave.SFCount;
		currSpawn.SSCount = AWave.SSCount;
		currSpawn.BCCount = AWave.BCCount;
		currSpawn.GDCount = AWave.GDCount;
		currSpawn.MOCount = AWave.MOCount;
		currSpawn.MSCount = AWave.MSCount;
		currSpawn.StartTime = AWave.StartTime;
		currSpawn.SpawnDelay = AWave.SpawnDelay;
		
		//Reset LastEnemySpawn
		lastEnemySpawn.TimeStamp = Timer;
		lastEnemySpawn.XPos = 0xFFFFF;
		lastEnemySpawn.XSize = 0;
	}
	
	
	/*=========================================================
	
		Returns a Random enemy of the given Wave to be Spawned.
		Also removes that enemy from the Wave.
	
	=========================================================*/	
	GameObject GetRandomEnemy(){
		//Motherships should always Spawn first
		if (currSpawn.MSCount > 0){
			currSpawn.MSCount--;
			return MS_Enemy;	
		}
		
		//Check if any Enemy can spawn
		int EnemyCount = currSpawn.BCCount + currSpawn.BDCount + currSpawn.GDCount + currSpawn.MOCount + currSpawn.SFCount + currSpawn.SSCount;
		
		//crap1 = currSpawn.BDCount;
		
		if (EnemyCount <= 0) return null;
		
		int x = (int) (Random.value * EnemyCount);
		
		
		if (x < currSpawn.BDCount){
				if (currSpawn.BDCount > 0){
					currSpawn.BDCount--;
					return BD_Enemy;
				}
		}
		x -= currSpawn.BDCount;
		
		if (x < currSpawn.SFCount){
				if (currSpawn.SFCount > 0){
					currSpawn.SFCount--;
					return SF_Enemy;
				}
		}
		x -= currSpawn.SFCount;
		
		if (x < currSpawn.SSCount){
				if (currSpawn.SSCount > 0){
					currSpawn.SSCount--;
					return SS_Enemy;
				}
		}
		x -= currSpawn.SSCount;
		
		if (x < currSpawn.BCCount){
				if (currSpawn.BCCount > 0){
					currSpawn.BCCount--;
					return BC_Enemy;
				}
		}
		x -= currSpawn.BCCount;
		
		if (x < currSpawn.GDCount){
				if (currSpawn.GDCount > 0){
					currSpawn.GDCount--;
					return GD_Enemy;
				}
		}
		x -= currSpawn.GDCount;
		
		if (x < currSpawn.MOCount){
				if (currSpawn.MOCount > 0){
					currSpawn.MOCount--;
					return MO_Enemy;
				}
		}
		x -= currSpawn.MOCount;
		return null;
	}
	
	/*=========================================================
	
		Checks if an Enemy from the current Wave should be spawned, and if so, spawns it.
	
	=========================================================*/	
	void SpawnUpdate () {
		//If the time is < 0, its an invalid one / there is no wave spawning at the moment
		if (currSpawn.StartTime < 0) return;
		
		if (Timer - lastEnemySpawn.TimeStamp > currSpawn.SpawnDelay){
			GameObject toSpawn = GetRandomEnemy();
			if (toSpawn != null){
				crap1 = 1;
				//Get an X-Offset, that won't collide with other enemys
				float xVal = Random.value * xVarying - xVarying / 2;
				int runs = 0;
				while (Mathf.Abs(xVal - lastEnemySpawn.XPos) < Mathf.Abs((toSpawn.collider.bounds.size.x + lastEnemySpawn.XSize) / 2)) {
				xVal = Random.value * xVarying - xVarying / 2;
				runs++;
				if (runs > 20) break;
				}
				GameObject clone = Instantiate(toSpawn, StartVec + new Vector3(xVal, 0, 0), toSpawn.transform.rotation) as GameObject;	
				lastEnemySpawn.XPos = xVal;
				lastEnemySpawn.XSize = clone.collider.bounds.size.x;
				lastEnemySpawn.TimeStamp = Timer;
			} else {
				//There is no more enemy to Spawn.
				currSpawn.StartTime = - 1;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime * 1000;
		if ((Timer > Waves[LastSpawn+1].StartTime) && (Waves[LastSpawn+1].StartTime != - 1))  {
			SpawnWave(Waves[++LastSpawn]);
		}
		SpawnUpdate();
	}
}