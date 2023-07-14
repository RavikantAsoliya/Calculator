using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Calculator.Custom_Controls
{
    public class CustomTextBox : TextBox
    {
        private const int TextLimit = 21;

        public CustomTextBox()
        {
            TextChanged += CustomTextBox_TextChanged;
        }

        private void CustomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Remove commas from the text
            string textWithoutCommas = Text.Replace(",", string.Empty);

            // Truncate the text if it exceeds the limit
            if (textWithoutCommas.Length > TextLimit)
            {
                textWithoutCommas = textWithoutCommas.Substring(0, TextLimit);
                Select(TextLimit, 0); // Move the cursor to the end of the truncated text
            }

            // Add commas after every three characters
            string formattedText = AddCommas(textWithoutCommas);

            // Set the formatted text back to the TextBox
            Text = formattedText;

            // Adjust font size if text exceeds control's width
            double availableWidth = ActualWidth - Padding.Left - Padding.Right;
            double textWidth = MeasureTextWidth(formattedText);

            if (textWidth > availableWidth)
            {
                double fontSize = FontSize;

                // Decrease font size until the text fits within the available width
                while (textWidth > availableWidth && fontSize > 1)
                {
                    fontSize--;
                    SetFontSize(fontSize);
                    textWidth = MeasureTextWidth(formattedText);
                }
            }

            // Set FlowDirection to RightToLeft to display text from right to left
            FlowDirection = FlowDirection.RightToLeft;
        }

        private double MeasureTextWidth(string text)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                FontSize,
                Foreground,
                new NumberSubstitution(),
                VisualTreeHelper.GetDpi(this).PixelsPerDip
            );

            return formattedText.Width;
        }

        private void SetFontSize(double fontSize)
        {
            FontSize = fontSize;
        }

        private string AddCommas(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Reverse the text to add commas from right to left
            char[] charArray = text.ToCharArray();
            System.Array.Reverse(charArray);
            string reversedText = new string(charArray);

            // Add commas after every three characters
            string formattedText = string.Empty;
            for (int i = 0; i < reversedText.Length; i += 3)
            {
                int length = System.Math.Min(3, reversedText.Length - i);
                formattedText += reversedText.Substring(i, length);
                if (i + length < reversedText.Length)
                    formattedText += ",";
            }

            // Reverse the formatted text back to the original order
            charArray = formattedText.ToCharArray();
            System.Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}

