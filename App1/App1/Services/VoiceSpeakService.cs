using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;

namespace App1.Services;
public static class VoiceSpeakService
{
    private static SpeechSynthesizer synthesizer;
    private static ResourceContext speechContext;
    private static ResourceMap speechResourceMap;
    private static MediaPlayerElement  media = new MediaPlayerElement();
    /// <summary>
    /// 语音初始化
    /// </summary>
    public static void VoiceInit()
    {
        Scenario_SynthesizeTextBoundaries();
        InitializeListboxVoiceChooser();
        try
        {
            //var voices = SpeechSynthesizer.AllVoices;
            //foreach (VoiceInformation voice in voices.OrderBy(p => p.Language))
            //{
            //    if (reader.Contains(voice.DisplayName))
            //    {
            //        synthesizer.Voice = voice;
            //    }
            //}
        }
        catch
        {

        }
        try
        {
            //synthesizer.Options.SpeakingRate = double.Parse(await FileService.ReadFileStorage("", "VoiceSpeed"));
        }
        catch
        {
            //FileService.WriteFile("", "VoiceSpeed", "1.0");
        }
    }
    /// <summary>
     /// 朗读暂停
     /// </summary>
     /// <returns></returns>
    public static void ReadPause()
    {
        media.MediaPlayer.Pause();
    }
    /// <summary>
    /// 朗读开始
    /// </summary>
    /// <returns></returns>
    public static void ReadPlay()
    {
        media.MediaPlayer.Play();
    }
    /// <summary>
    /// 音量
    /// </summary>
    /// <returns></returns>
    public static double GetCurrentVolume()
    {
        return synthesizer.Options.AudioVolume;
    }
    /// <summary>
    /// 音量
    /// </summary>
    /// <param name="rate"></param>
    public static void SetCurrentVolume(double rate)
    {
        synthesizer.Options.AudioVolume = rate;
    }
    /// <summary>
    /// 音调
    /// </summary>
    /// <returns></returns>
    public static double GetCurrentPitch()
    {
        return synthesizer.Options.AudioPitch; 
    }
    /// <summary>
    /// 音调
    /// </summary>
    /// <returns></returns>
    public static void SetCurrentPitch(double rate)
    {
        synthesizer.Options.AudioPitch = rate;
    }
    /// <summary>
    /// 读速
    /// </summary>
    /// <returns></returns>
    public static double GetCurrentSpeed()
    {
        return synthesizer.Options.SpeakingRate;
    }
    /// <summary>
    /// 读速
    /// </summary>
    /// <returns></returns>
    public static void SetCurrentSpeed(double rate)
    {
        synthesizer.Options.SpeakingRate=rate;
    }
    /// <summary>
    /// 当前按朗读者
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentReader()
    {
        VoiceInformation currentVoice = synthesizer.Voice;
        return currentVoice.Language;
    }
    /// <summary>
    /// 当前按朗读者
    /// </summary>
    /// <returns></returns>
    public static void SetCurrentReader(VoiceInformation reader)
    {
        synthesizer.Voice = reader;
    }
    /// <summary>
    /// 朗读者列表
    /// </summary>
    /// <returns></returns>
    public static List<VoiceInformation> GetReaders()
    {
        return _voicechooser;
    }
    /// <summary>
    /// 朗读文本
    /// </summary>
    /// <param name="text"></param>
    static public async void Speak(string text)
    {
        // If the media is playing, the user has pressed the button to stop the playback.
        //if (media.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
        //{
        //    media.MediaPlayer.Pause();
        //}
        //else
        {

            try
            {
                // Enable word marker generation (false by default). 
                synthesizer.Options.IncludeWordBoundaryMetadata = true;
                synthesizer.Options.IncludeSentenceBoundaryMetadata = true;

                SpeechSynthesisStream synthesisStream = await synthesizer.SynthesizeTextToStreamAsync(text);

                // Create a media source from the stream: 
                var mediaSource = Windows.Media.Core.MediaSource.CreateFromStream(synthesisStream, synthesisStream.ContentType);

                //Create a Media Playback Item   
                var mediaPlaybackItem = new MediaPlaybackItem(mediaSource);

                // Ensure that the app is notified for cues.  
                RegisterForWordBoundaryEvents(mediaPlaybackItem);

                // Set the source of the MediaElement or MediaPlayerElement to the MediaPlaybackItem 
                // and start playing the synthesized audio stream.
                media.Source = mediaPlaybackItem;
                media.MediaPlayer.Play();
            }
            catch (System.IO.FileNotFoundException)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components unavailable");
                await messageDialog.ShowAsync();
            }
            catch (Exception)
            {
                media.AutoPlay = false;
                var messageDialog = new Windows.UI.Popups.MessageDialog("Unable to synthesize text");
                await messageDialog.ShowAsync();
            }
        }
    }
    static void Scenario_SynthesizeTextBoundaries()
    {
        synthesizer = new SpeechSynthesizer();
        speechContext = ResourceContext.GetForCurrentView();
        speechContext.Languages = new string[] { SpeechSynthesizer.DefaultVoice.Language };
        speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationTTSResources");
        MediaPlayer player = new MediaPlayer();
        player.AutoPlay = false;
        player.MediaEnded += media_MediaEnded;
        media.SetMediaPlayer(player);
        InitializeListboxVoiceChooser();
    }
    static List<VoiceInformation> _voicechooser = new List<VoiceInformation>();
    private static void InitializeListboxVoiceChooser()
    {
        _voicechooser.Clear();
        // Get all of the installed voices.
        var voices = SpeechSynthesizer.AllVoices;
        // Get the currently selected voice.
        VoiceInformation currentVoice = synthesizer.Voice;
        foreach (VoiceInformation voice in voices.OrderBy(p => p.Language))
        {
            _voicechooser.Add(voice);
        }
    }
    static async void media_MediaEnded(MediaPlayer sender, object e)
    {
        media.MediaPlayer.Pause();
    }
    /// <summary>
    /// Register for just sentence boundary events.
    /// </summary>
    /// <param name="mediaPlaybackItem">The Media Playback Item to register handlers for.</param>
    /// <param name="index">Index of the timedMetadataTrack within the mediaPlaybackItem.</param>
    static private void RegisterMetadataHandlerForSentences(MediaPlaybackItem mediaPlaybackItem, int index)
    {
        var timedTrack = mediaPlaybackItem.TimedMetadataTracks[index];
        if (timedTrack.Id == "SpeechSentence")
        {
            timedTrack.CueEntered += metadata_SpeechCueEntered;
            mediaPlaybackItem.TimedMetadataTracks.SetPresentationMode((uint)index, TimedMetadataTrackPresentationMode.ApplicationPresented);
        }
    }
    static private async void metadata_SpeechCueEntered(Windows.Media.Core.TimedMetadataTrack timedMetadataTrack, Windows.Media.Core.MediaCueEventArgs args)
    {
        // Check in case there are different tracks and the handler was used for more tracks 
        if (timedMetadataTrack.TimedMetadataKind == Windows.Media.Core.TimedMetadataKind.Speech)
        {
            var cue = args.Cue as Windows.Media.Core.SpeechCue;
            if (cue != null)
            {
                System.Diagnostics.Debug.WriteLine("Hit Cue with start:" + cue.StartPositionInInput + " and end:" + cue.EndPositionInInput);
                System.Diagnostics.Debug.WriteLine("Cue text:[" + cue.Text + "]");
                // Do something with the cue 

                //FillTextBoxes(cue, timedMetadataTrack);
            }
        }
    }
    static private void RegisterForWordBoundaryEvents(MediaPlaybackItem mediaPlaybackItem)
    {
        // If tracks were available at source resolution time, itterate through and register: 
        for (int index = 0; index < mediaPlaybackItem.TimedMetadataTracks.Count; index++)
        {
            RegisterMetadataHandlerForWords(mediaPlaybackItem, index);
            RegisterMetadataHandlerForSentences(mediaPlaybackItem, index);
        }

        // Since the tracks are added later we will  
        // monitor the tracks being added and subscribe to the ones of interest 
        mediaPlaybackItem.TimedMetadataTracksChanged += (MediaPlaybackItem sender, IVectorChangedEventArgs args) =>
        {
            if (args.CollectionChange == CollectionChange.ItemInserted)
            {
                RegisterMetadataHandlerForWords(sender, (int)args.Index);
                RegisterMetadataHandlerForSentences(mediaPlaybackItem, (int)args.Index);
            }
            else if (args.CollectionChange == CollectionChange.Reset)
            {
                for (int index = 0; index < sender.TimedMetadataTracks.Count; index++)
                {
                    RegisterMetadataHandlerForWords(sender, index);
                    RegisterMetadataHandlerForSentences(mediaPlaybackItem, index);
                }
            }
        };
    }
    /// <summary>
    /// Register for just word boundary events.
    /// </summary>
    /// <param name="mediaPlaybackItem">The Media Playback Item to register handlers for.</param>
    /// <param name="index">Index of the timedMetadataTrack within the mediaPlaybackItem.</param>
    static private void RegisterMetadataHandlerForWords(MediaPlaybackItem mediaPlaybackItem, int index)
    {
        var timedTrack = mediaPlaybackItem.TimedMetadataTracks[index];
        //register for only word cues
        if (timedTrack.Id == "SpeechWord")
        {
            timedTrack.CueEntered += metadata_SpeechCueEntered;
            mediaPlaybackItem.TimedMetadataTracks.SetPresentationMode((uint)index, TimedMetadataTrackPresentationMode.ApplicationPresented);
        }
    }
   
}
