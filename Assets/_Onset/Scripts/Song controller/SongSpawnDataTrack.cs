using System;
using System.Collections.Generic;

[Serializable]
public class SongSpawnDataTrack : SongSpawnData
{
    private List<TrackPath> currentTrackPaths = new List<TrackPath>();
    public List<TrackPath> CurrentTrackPaths => currentTrackPaths;
    public override void Initialize()
    {
        base.Initialize();

        currentSongSpawnData.Tracks.ForEach(track => currentTrackPaths.Add(track as TrackPath));

        timeToHitFirstTrack = currentSongSpawnData.Tracks[0].DistanceToHitPoint / currentSongSpawnData.NoteSpeed;
        currentSongSpawnData.Tracks.ForEach(track => track.InitTarget());
    }

    public override float TimeToHit(Track track) => track.DistanceToHitPoint / GetSpeedToMatchFirstTrack(track);


}
