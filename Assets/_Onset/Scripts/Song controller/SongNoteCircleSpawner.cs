using UnityEngine;

public class SongNoteCircleSpawner : SongNoteSpawner
{
    [SerializeField]
    private SongSpawnDataCircle songSpawnDataCircle;

    protected override void Awake()
    {
        base.Awake();
        songSpawnData = songSpawnDataCircle;
    }
    protected override void SongLoader_OnFinishLoadingSong()
    {
        base.SongLoader_OnFinishLoadingSong();

        timeToHitAudioSettings = AudioSettings.dspTime + songSpawnDataCircle.TimeToHitFirstTrack;
    }

    protected override void SpawnNote()
    {
        GameObject noteGameObjectInstance = songSpawnDataCircle.NotePool.GetPrefabFromPool();

        noteGameObjectInstance.transform.position = songSpawnDataCircle.CurrentSongSpawnData.Tracks[currentTrackIndex].transform.position;

        CircleScaler circleScalerInstance = noteGameObjectInstance.GetComponent<CircleScaler>();
        circleScalerInstance.Setup(songSpawnDataCircle.TimeToHitFirstTrack);

        Note noteInstance = noteGameObjectInstance.GetComponent<Note>();
        noteInstance.OnMissNote -= NoteInstance_OnMissHitPoint;
        noteInstance.OnMissNote += NoteInstance_OnMissHitPoint;
        noteInstance.Setup(songSpawnDataCircle.NotePool, songSpawnDataCircle.TimeToHit(songSpawnDataCircle.CurrentSongSpawnData.Tracks[currentTrackIndex]));
    }
}
