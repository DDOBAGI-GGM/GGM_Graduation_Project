using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";         
    // ���Խ�, ',' �޸� �������� ������. ��� ���ڿ����� ��ġ�� ã�µ� "" �� �ѷ����� �޸��� ��ġ�� ����. (Ex. ",") �� �� �ֵ���ǥ�� ������ ���� �� ����.
    static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";       // 4���� �ٹٲ� �����߿��� �ϳ��� ������. | �� ����.
    static readonly char[] TRIM_CHARS = { '\"' };
    // static �� ��� �� Ŭ������ �����ص� �� ������ ��� ���ϱ� ����. @�� C# ���� escape ���ڸ� �������ִ� ������. readonly �� �� ���� ���� �����.

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();      // ����Ʈ ��ųʸ�... ��... ��... 2����...?
        TextAsset data = Resources.Load(file) as TextAsset;     // �ؽ�Ʈ ������ �������ִ� ��. �̸����� �������ְ� TextAsset ���� �־���. ������ null;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);      // ���Խ��� ����ؼ� ���� ������.

        if (lines.Length <= 1) return list;     // 1�� �� ũ�� ����.  ����� �ִ� ��� ����

        var header = Regex.Split(lines[0], SPLIT_RE);       // �ش�, �Ӹ��� �ִ� ������ ��������. , �������� �ؼ�

        for (int i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);               // ��� ���� ù��° �� ���� , �������� ������.
            if (values.Length == 0 || values[0] == "") continue;        // ù��°�� ����ų� ���̰� 0�̸� ���� ���� ����.

            var entry = new Dictionary<string, object>();
            for (int j = 0; j < header.Length && j < values.Length; j++)        // ù��° ���� �� ���̺��� �۰� ����� ���̰� 0�� �ƴ� ��
            {
                string value = values[j];       // value ������.
                //value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                // �װ� �ֵ���ǥ�� �ѷ��׿��ְų� �齽������ �κ��� ������ �������� �־����. �츮�� �����Ϳ����� �ʿ����.

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
                entry[header[j]] = finalvalue;          // ��ųʸ�, header �� �� ������ �־��ֱ�. 
            }

            list.Add(entry);        // ��ųʸ��� �־��� �� �߰����ֱ�. ���� for ���̶� ��� �޶����ϱ�.
        }
        return list;
    }

    public static List<List<string>> IngrediendRead()       // ��� ��ȯ
    {
        TextAsset data = Resources.Load("CSV/Recipe") as TextAsset;     // �ؽ�Ʈ ������ �������ִ� ��. �̸����� �������ְ� TextAsset ���� �־���. ������ null;

        Debug.Log(data);

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);      // ���Խ��� ����ؼ� ���� ������.

        List<List<string>> list = new List<List<string>>();
        for (int i = 1; i < lines.Length; i++)      // ����� �����ϰ�
        {
            list.Add(new List<string>());
        }

        if (lines.Length <= 1) return list;     // 1�� �� ũ�� ����.  ����� �ִ� ��� ����

        int headerCnt = Regex.Split(lines[0], SPLIT_RE).Length;       // �ش�, �Ӹ��� �ִ� ������ ��������. , �������� �ؼ�

        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] values = Regex.Split(lines[i + 1], SPLIT_RE);               // ��� ���� ù��° �� ���� , �������� ������.
            if (values.Length == 0 || values[0] == "") continue;        // ù��°�� ����ų� ���̰� 0�̸� ���� ���� ����.

            for (int j = 0; j < values.Length; j++)
            {
                list[i].Add(values[j]);
            }
        }

        return list;
    }
}

// https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/#comment-7111         CSV �ڵ�
// https://blog.naver.com/nicecapj/220451022167          Resources.Load() �� �ǽð��� ����ϴ� ���� �ſ� ���ڹǷ� ĳ���ؼ� ���������.