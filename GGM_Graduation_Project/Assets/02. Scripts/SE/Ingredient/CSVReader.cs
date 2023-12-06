using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";         
    // 정규식, ',' 콤마 기준으로 나눠줌. 모든 문자열에서 매치를 찾는데 "" 에 둘러쌓인 콤마는 매치를 안함. (Ex. ",") 또 이 쌍따옴표는 여러번 나올 수 있음.
    static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";       // 4개의 줄바꿈 문자중에서 하나라도 나오면. | 로 구분.
    static readonly char[] TRIM_CHARS = { '\"' };
    // static 은 어디서 이 클래스를 생성해도 이 변수는 계속 쓰니까 붙임. @는 C# 에서 escape 문자를 무시해주는 역할임. readonly 는 값 변경 되지 말라고.

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();      // 리스트 딕셔너리... 어... 뭐... 2차원...?
        TextAsset data = Resources.Load(file) as TextAsset;     // 텍스트 에셋을 가져와주는 것. 이름으로 가져와주고 TextAsset 으로 넣어줌. 없으면 null;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);      // 정규식을 사용해서 줄을 나눠줌.

        if (lines.Length <= 1) return list;     // 1이 더 크면 리턴.  헤더만 있는 경우 배제

        var header = Regex.Split(lines[0], SPLIT_RE);       // 해더, 머리에 있는 값들을 가져와줌. , 기준으로 해서

        for (int i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);               // 헤더 빼고 첫번째 것 부터 , 기준으로 나눠줌.
            if (values.Length == 0 || values[0] == "") continue;        // 첫번째가 비었거나 길이가 0이면 값이 없는 것임.

            var entry = new Dictionary<string, object>();
            for (int j = 0; j < header.Length && j < values.Length; j++)        // 첫번째 줄이 총 길이보다 작고 헤더가 길이가 0이 아닐 때
            {
                string value = values[j];       // value 돌아줌.
                //value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                // 그거 쌍따옴표로 둘러쌓여있거나 백슬래시인 부분이 있으면 공백으로 넣어줘라. 우리의 데이터에서는 필요없음.

                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;          // 딕셔너리, header 에 각 값들을 넣어주기. 
            }

            list.Add(entry);        // 딕셔너리에 넣어준 것 추가해주기. 위에 for 문이라 계속 달라지니까.
        }
        return list;
    }

    public static List<List<string>> IngrediendRead()       // 재료 반환
    {
        TextAsset data = Resources.Load("CSV/Recipe") as TextAsset;     // 텍스트 에셋을 가져와주는 것. 이름으로 가져와주고 TextAsset 으로 넣어줌. 없으면 null;

        Debug.Log(data);

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);      // 정규식을 사용해서 줄을 나눠줌.

        List<List<string>> list = new List<List<string>>();
        for (int i = 1; i < lines.Length; i++)      // 헤더는 제외하고
        {
            list.Add(new List<string>());
        }

        if (lines.Length <= 1) return list;     // 1이 더 크면 리턴.  헤더만 있는 경우 배제

        int headerCnt = Regex.Split(lines[0], SPLIT_RE).Length;       // 해더, 머리에 있는 값들을 가져와줌. , 기준으로 해서

        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] values = Regex.Split(lines[i + 1], SPLIT_RE);               // 헤더 빼고 첫번째 것 부터 , 기준으로 나눠줌.
            if (values.Length == 0 || values[0] == "") continue;        // 첫번째가 비었거나 길이가 0이면 값이 없는 것임.

            for (int j = 0; j < values.Length; j++)
            {
                list[i].Add(values[j]);
            }
        }

        return list;
    }
}

// https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/#comment-7111         CSV 코드
// https://blog.naver.com/nicecapj/220451022167          Resources.Load() 는 실시간에 사용하는 것은 매우 나쁘므로 캐싱해서 사용해주자.