using System.Text;

public static class RichTextUtility
{
    // ------------------------------
    // STRIP TAGS
    // ------------------------------
    public static string WrapInMonospaceTag(string richText, string value)
    {
        StringBuilder result = new StringBuilder(richText);
        result.Insert(0, "<mspace=" + value + ">");
        result.Append("</mspace>");
        return result.ToString();
    }
    
    // ------------------------------
    // STRIP TAGS
    // ------------------------------
    public static string StripTags(string richText)
    {
        if (string.IsNullOrEmpty(richText))
            return string.Empty;

        StringBuilder result = new StringBuilder(richText.Length);
        bool insideTag = false;

        foreach (char c in richText)
        {
            if (c == '<') { insideTag = true; continue; }
            if (c == '>') { insideTag = false; continue; }

            if (!insideTag)
                result.Append(c);
        }

        return result.ToString();
    }

    // ------------------------------
    // FIND RAW INDEX OF VISIBLE CHARACTER
    // ------------------------------
    public static int GetRichTextIndexOfVisibleChar(string richText, int visibleIndex)
    {
        if (string.IsNullOrEmpty(richText) || visibleIndex < 0)
            return -1;

        int visibleCount = 0;
        bool insideTag = false;

        for (int i = 0; i < richText.Length; i++)
        {
            char c = richText[i];

            if (c == '<')
            {
                insideTag = true;
                continue;
            }

            if (c == '>')
            {
                insideTag = false;
                continue;
            }

            if (!insideTag)
            {
                if (visibleCount == visibleIndex)
                    return i;

                visibleCount++;
            }
        }

        return -1;
    }

    // ------------------------------
    // GET SPAN OF VISIBLE CHARACTER INCLUDING ITS TAGS
    // ------------------------------
    public static (int start, int end) GetRichTextSpanOfVisibleChar(string richText, int visibleIndex)
    {
        int start = GetRichTextIndexOfVisibleChar(richText, visibleIndex);
        if (start == -1)
            return (-1, -1);

        int end = start + 1;

        bool insideTag = false;

        for (int i = start + 1; i < richText.Length; i++)
        {
            char c = richText[i];

            if (c == '<')
                insideTag = true;
            else if (c == '>')
                insideTag = false;
            else if (!insideTag)
                break;

            end = i + 1;
        }

        return (start, end);
    }

    // ------------------------------
    // NEW: SET COLOR OF A VISIBLE CHARACTER
    // ------------------------------
    public static string SetColorOfVisibleChar(string richText, int visibleIndex, string color)
    {
        if (string.IsNullOrEmpty(richText))
            return richText;

        var (start, end) = GetRichTextSpanOfVisibleChar(richText, visibleIndex);
        if (start == -1)
            return richText;

        string inner = richText.Substring(start, end - start);

        // Remove any existing <color> tag around this character
        string cleaned = RemoveOuterColorTag(inner);

        // Wrap the cleaned character span in the new color tag
        string recolored = $"<color={color}>{cleaned}</color>";

        // Build the final rich-text string
        StringBuilder result = new StringBuilder(richText.Length + 16);
        result.Append(richText.Substring(0, start));
        result.Append(recolored);
        result.Append(richText.Substring(end));

        return result.ToString();
    }
    
    // ------------------------------
    // NEW: SET COLOR OF A VISIBLE CHARACTER
    // ------------------------------
    public static string SetColorOfRichText(string richText, string color)
    {
        if (string.IsNullOrEmpty(richText))
            return richText;
        
        // Remove any existing <color> tag around this character
        string cleaned = RemoveOuterColorTag(richText);

        // Wrap the cleaned character span in the new color tag
        string recolored = $"<color={color}>{cleaned}</color>";

        return recolored;
    }

    // Helper: removes a single outer <color=...> tag if present
    private static string RemoveOuterColorTag(string text)
    {
        if (text.StartsWith("<color="))
        {
            int close = text.IndexOf('>');
            if (close != -1)
            {
                // Remove the opening tag
                text = text.Substring(close + 1);

                // Remove the matching closing </color> tag if present
                if (text.EndsWith("</color>"))
                {
                    text = text.Substring(0, text.Length - "</color>".Length);
                }
            }
        }
        return text;
    }

    // ------------------------------
    // COUNT VISIBLE CHARACTERS
    // ------------------------------
    public static int GetVisibleLength(string richText)
    {
        int count = 0;
        bool insideTag = false;

        foreach (char c in richText)
        {
            if (c == '<') { insideTag = true; continue; }
            if (c == '>') { insideTag = false; continue; }

            if (!insideTag)
                count++;
        }

        return count;
    }
}
