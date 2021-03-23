using Common.DataStructures.Coordinates;
using NUnit.Framework;
using System;

namespace TestCommon.DataStructures.Coordinates
{
    class VectorTestSuite
    {
        const double DEGREES_45 = Math.PI / 4;
        const double DEGREES_90 = Math.PI / 2;
        const double DEGREES_135 = DEGREES_45 + DEGREES_90;
        const double DEGREES_225 = DEGREES_45 + 2 * DEGREES_90;
        const double DEGREES_315 = DEGREES_45 + 3 * DEGREES_90;
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateWithAngleAndModule_FirstQuarter()
        {
            var vector = Vector.CreateWithAngleAndModule(DEGREES_45);
            Assert.IsTrue(vector.X > 0);
            Assert.IsTrue(vector.Y > 0);
        }
        [Test]
        public void CreateWithAngleAndModule_SecondQuarter()
        {
            var vector = Vector.CreateWithAngleAndModule(DEGREES_135);
            Assert.IsTrue(vector.X < 0);
            Assert.IsTrue(vector.Y > 0);
        }
        [Test]
        public void CreateWithAngleAndModule_ThirdQuarter()
        {
            var vector = Vector.CreateWithAngleAndModule(DEGREES_225);
            Assert.IsTrue(vector.X < 0);
            Assert.IsTrue(vector.Y < 0);
        }
        [Test]
        public void CreateWithAngleAndModule_FourthQuarter()
        {
            var vector = Vector.CreateWithAngleAndModule(DEGREES_315);
            Assert.IsTrue(vector.X > 0);
            Assert.IsTrue(vector.Y < 0);
        }
    }
}
