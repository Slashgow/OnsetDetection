using System.Collections.Generic;
using UnityEngine;

public class SongNoteTrackPathSpawner : SongNoteSpawner
{
    [SerializeField]
    protected SongSpawnDataTrack songSpawnDataTrack;

    protected override void Awake()
    {
        base.Awake();
        songSpawnData = songSpawnDataTrack;
    }
    protected override void SpawnNote()
    {
        GameObject noteGameObjectInstance = songSpawnDataTrack.NotePool.GetPrefabFromPool();
    
        TrackFollower trackFollowerInstance = noteGameObjectInstance.GetComponent<TrackFollower>();
        float noteSpeed = currentTrackIndex == 0 ? songSpawnDataTrack.CurrentSongSpawnData.NoteSpeed : songSpawnDataTrack.GetSpeedToMatchFirstTrack(songSpawnData.CurrentSongSpawnData.Tracks[currentTrackIndex]);
        trackFollowerInstance.Setup(songSpawnDataTrack.CurrentTrackPaths[currentTrackIndex].PathCreatorToHitNote,
            songSpawnDataTrack.CurrentTrackPaths[currentTrackIndex].PathCreatorFromHitNoteToPlanet, 
            noteSpeed);
    
        Note noteInstance = noteGameObjectInstance.GetComponent<Note>();
        noteInstance.OnMissNote -= NoteInstance_OnMissHitPoint;
        noteInstance.OnMissNote += NoteInstance_OnMissHitPoint;
        noteInstance.Setup(songSpawnDataTrack.NotePool, songSpawnDataTrack.TimeToHit(songSpawnDataTrack.CurrentSongSpawnData.Tracks[currentTrackIndex]));
    }
}
