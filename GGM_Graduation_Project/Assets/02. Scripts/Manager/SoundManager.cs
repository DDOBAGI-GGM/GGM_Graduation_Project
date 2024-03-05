using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}

public class SoundManager : Singleton<SoundManager>
{
    public Sound[] bgmSounds;           // BGM 사운드 저장
    public Sound[] effectSounds;        // SFX 사운드 저장

    public AudioSource audioSourceBgmPlayers;           // BGM을 출력할 오디오 소스
    public AudioSource audioSourceEffectsPlayers;     // SFX를 출력할 오디오 소스

    private void Start()
    {
        PlayBGM("intro");
    }

    public void PlayBGM(string name) // BGM 실행
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (name == bgmSounds[i].soundName)
            {
                audioSourceBgmPlayers.clip = bgmSounds[i].clip;
                audioSourceBgmPlayers.loop = true;
                audioSourceBgmPlayers.Play();
                return;
            }
        }
        //Debug.LogError("배경음악 사운드 이름 잘못부름!");
    }

    public void PlaySFX(string name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (name == effectSounds[i].soundName)
            {
                audioSourceEffectsPlayers.PlayOneShot(effectSounds[i].clip);        // 소리 한번만 딱 내주는 것.
                return;
            }
        }
        //Debug.LogError("이펙트 사운드 이름 잘못부름!");
    }


    public void StopBGM()
    {

        audioSourceBgmPlayers.Stop();
    }

    public void StopEffectsSound()
    {
        audioSourceEffectsPlayers.Stop();
    }
}