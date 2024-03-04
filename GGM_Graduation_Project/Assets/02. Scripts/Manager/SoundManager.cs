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
    public Sound[] bgmSounds;           // BGM ���� ����
    public Sound[] effectSounds;        // SFX ���� ����

    public AudioSource audioSourceBgmPlayers;           // BGM�� ����� ����� �ҽ�
    public AudioSource audioSourceEffectsPlayers;     // SFX�� ����� ����� �ҽ�

    private void Start()
    {
        PlayBGM("intro");
    }

    public void PlayBGM(string name) // BGM ����
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
        //Debug.LogError("������� ���� �̸� �߸��θ�!");
    }

    public void PlaySFX(string name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (name == effectSounds[i].soundName)
            {
                audioSourceEffectsPlayers.PlayOneShot(effectSounds[i].clip);        // �Ҹ� �ѹ��� �� ���ִ� ��.
                return;
            }
        }
        //Debug.LogError("����Ʈ ���� �̸� �߸��θ�!");
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