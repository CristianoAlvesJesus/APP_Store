using CommonTestUtilities.Requests;
using System.Collections;

namespace Store.UseCase.Test.Transaction.InlineDatas;

public class ImagesTypesInlineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var images = FormFileBuilder.ImageCollections();
        foreach (var image in images)
            yield return new object[] { image };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
