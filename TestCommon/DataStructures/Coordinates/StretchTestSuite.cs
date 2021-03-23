using NUnit.Framework;
using Common.DataStructures.Coordinates;

namespace TestCommon.DataStructures.Coordinates
{
    public class StretchTestSuite
    {
        Stretch stretch;
        Location pointOnStretch, pointOnEnd, pointOnStart, pointOnLineButOutside, pointOnLeft, pointOnRight;

        [SetUp]
        public void Setup()
        {
            pointOnStart = new Location(0, 0);
            pointOnEnd = new Location(2, 2);
            pointOnStretch = new Location(1, 1);
            pointOnLineButOutside = new Location(-1, -1);
            pointOnLeft = new Location(0, 1);
            pointOnRight = new Location(1, 0);
            stretch = new Stretch(pointOnStart, pointOnEnd);
        }

        // Contains
        [Test]
        public void Contains_ShouldReturnTrue_WhenPointBelongs()
        {
            Assert.IsTrue(stretch.Contains(pointOnStretch));
        }
        [Test]
        public void Contains_ShouldReturnTrue_WhenPointOnStart()
        {
            Assert.IsTrue(stretch.Contains(pointOnStart));
        }
        [Test]
        public void Contains_ShouldReturnTrue_WhenPointOnEnd()
        {
            Assert.IsTrue(stretch.Contains(pointOnEnd));
        }
        [Test]
        public void Contains_ShouldReturnFalse_WhenPointOnLineButNotOnStretch()
        {
            Assert.IsFalse(stretch.Contains(pointOnLineButOutside));
        }
        [Test]
        public void Contains_ShouldReturnFalse_WhenPointNotOnLine()
        {
            Assert.IsFalse(stretch.Contains(pointOnLeft));
        }

        //  OnTheSameLine
        [Test]
        public void BelongsToLine_ShouldReturnTrue_WhenPointBelongsToStretch()
        {
            Assert.IsTrue(stretch.OnTheSameLine(pointOnStretch));
        }
        [Test]
        public void BelongsToLine_ShouldReturnTrue_WhenPointOnStart()
        {
            Assert.IsTrue(stretch.OnTheSameLine(pointOnStart));
        }
        [Test]
        public void BelongsToLine_ShouldReturnTrue_WhenPointOnEnd()
        {
            Assert.IsTrue(stretch.OnTheSameLine(pointOnEnd));
        }
        [Test]
        public void BelongsToLine_ShouldReturnTrue_WhenPointOnLineButNotOnStretch()
        {
            Assert.IsTrue(stretch.OnTheSameLine(pointOnLineButOutside));
        }
        [Test]
        public void BelongsToLine_ShouldReturnFalse_WhenPointNotOnLine()
        {
            Assert.IsFalse(stretch.OnTheSameLine(pointOnLeft));
        }
        [Test]
        public void BelongsToLine_ShouldReturnFalse_WhenStretchNotOnLine()
        {
            Assert.IsFalse(stretch.OnTheSameLine(new Stretch(pointOnLeft, pointOnRight)));
        }
        [Test]
        public void BelongsToLine_ShouldReturnTrue_WhenStretchOnLine()
        {
            Assert.IsTrue(stretch.OnTheSameLine(new Stretch(pointOnStretch, pointOnLineButOutside)));
        }
        [Test]
        public void BelongsToLine_ShouldReturnFalse_WhenStretchHasOneEndOnLine()
        {
            Assert.IsFalse(stretch.OnTheSameLine(new Stretch(pointOnStretch, pointOnRight)));
            Assert.IsFalse(stretch.OnTheSameLine(new Stretch(pointOnRight, pointOnStretch)));
        }

        // GetRelativePosition
        [Test]
        public void GetRelativePosition_Returns_ON_LINE()
        {
            Assert.AreEqual(stretch.GetRelativePosition(pointOnStretch), Stretch.RelativePosition.ON_LINE);
        }
        [Test]
        public void GetRelativePosition_Returns_RIGHT()
        {
            Assert.AreEqual(stretch.GetRelativePosition(pointOnRight), Stretch.RelativePosition.RIGHT);
        }
        [Test]
        public void GetRelativePosition_Returns_LEFT()
        {
            Assert.AreEqual(stretch.GetRelativePosition(pointOnLeft), Stretch.RelativePosition.LEFT);
        }

        // DirectionIntersectionWith
        [Test]
        public void DirectionIntersectionFound_WhenStretchesAreCrossing()
        {
            var crossing = new Location(0.5, 0.5);
            Assert.AreEqual(stretch.DirectionIntersectionWith(new Stretch(pointOnLeft, pointOnRight)), crossing);
        }
        [Test]
        public void DirectionIntersectionFound_WhenCrossingOutsideOneStretch()
        {
            var crossing = new Location(0.5, 0.5);
            Assert.AreEqual(stretch.DirectionIntersectionWith(new Stretch(pointOnLeft, new Location(-1, 2))), crossing);
        }
        [Test]
        public void DirectionIntersectionIsNull_WhenParallel()
        {
            Assert.AreEqual(stretch.DirectionIntersectionWith(new Stretch(pointOnLeft, new Location(1, 2))), null);
        }
    }
}

   