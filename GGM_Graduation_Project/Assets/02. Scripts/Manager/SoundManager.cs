using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;        // �̰� ���� �� �������� Ŭ���̸��� �ٲ㼭 �װɷ� �Ǵ��ϰ� �ϱ�!
    public AudioClip clip;
}

public class SoundManager : Singleton<SoundManager>
{
   /* static public SoundManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }*/
    public Sound[] bgmSounds;           // BGM ���� ����
    public Sound[] effectSounds;        // SFX ���� ����
    public AudioSource audioSourceBgmPlayers;           // BGM�� ����� ����� �ҽ�
    public AudioSource audioSourceEffectsPlayers;     // SFX�� ����� ����� �ҽ�
    //public string[] playSoundName;                      // �÷����� ���� �̸�

    private void Start()
    {
        //playSoundName = new string[audioSourceEffectsPlayers.Length];
        PlayBGM("Base");
    }

    public void PlayBGM(string name) // BGM ����
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (name == bgmSounds[i].soundName)
            {
                audioSourceBgmPlayers.clip = bgmSounds[i].clip;
                audioSourceBgmPlayers.Play();
                return;
            }
        }
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
    }

/*    public void StopBGM()
    {
        audioSourceBgmPlayers.Stop();
    }

    public void StopAllEffectsSound() // ��� SFX�� ����
    {
        for (int i = 0; i < audioSourceEffectsPlayers.Length; i++)
            audioSourceEffectsPlayers[i].Stop();
    }

    public void StopEffectsSound(string name) // Ư�� SFX�� ����
    {
        for (int i = 0; i < audioSourceEffectsPlayers.Length; i++)
        {
            if (playSoundName[i] == name)
            {
                audioSourceEffectsPlayers[i].Stop();
                break;
            }
        }
    }*/
}