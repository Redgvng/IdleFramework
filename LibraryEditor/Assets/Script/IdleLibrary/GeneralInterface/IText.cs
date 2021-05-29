using UnityEngine;
namespace IdleLibrary
{
    public interface IText
    {
        string Text();
    }

    public class NullText : IText
    {
        public string Text() => "";
    }

    //IText用デコレート
    public class ColorText : IText
    {
        private readonly IText text;
        private readonly Color color;
        public ColorText(IText text, Color color)
        {
            this.text = text;
            this.color = color;
        }
        public string Text()
        {
            if (this.text.Text() == "") return "";
            string colorCode = ColorUtility.ToHtmlStringRGB(color);
            string text = $"<color=#{colorCode}>{this.text.Text()}</color>";
            return text;
        }
    }

}