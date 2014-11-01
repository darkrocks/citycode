namespace Guide.Services.Contracts
{
    public interface ITextGenerator
    {
        string GenWords(int wordCount = 1, TextGenerator.SourceNames sourceNames = 0);

        string GenSentences(int sentenceCount = 1, TextGenerator.SourceNames sourceNames = 0);
    }

}
