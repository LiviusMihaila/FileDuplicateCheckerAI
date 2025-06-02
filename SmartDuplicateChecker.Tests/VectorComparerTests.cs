using SmartDuplicateChecker.API.Services;
using Xunit;

public class VectorComparerTests
{
    [Fact]
    public void CosineSimilarity_IdenticalVectors_Returns1()
    {
        var v1 = new float[] { 1f, 2f, 3f };
        var v2 = new float[] { 1f, 2f, 3f };

        var result = VectorComparer.CosineSimilarity(v1, v2);

        Assert.Equal(1.0, result, 3);
    }

    [Fact]
    public void CosineSimilarity_OrthogonalVectors_ReturnsZero()
    {
        var v1 = new float[] { 1f, 0f };
        var v2 = new float[] { 0f, 1f };

        var result = VectorComparer.CosineSimilarity(v1, v2);

        Assert.Equal(0.0, result, 3);
    }
}
