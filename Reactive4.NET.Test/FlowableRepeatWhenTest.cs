﻿using NUnit.Framework;
using System;
using System.Linq;

namespace Reactive4.NET.Test
{
    [TestFixture]
    public class FlowableRepeatWhenTest
    {
        [Test]
        public void Normal()
        {
            Flowable.Just(1)
                .RepeatWhen(f =>
                {
                    int[] counter = { 0 };

                    return f.TakeWhile(o => ++counter[0] < 5);
                })
                .Test()
                .AssertResult(1, 1, 1, 1, 1);
        }

        [Test]
        public void Tck102()
        {
            Flowable.Just(1)
                .RepeatWhen(f =>
                {
                    int[] counter = { 0 };

                    return f.TakeWhile(o => ++counter[0] < 3);
                })
                .Test(10)
                .AssertResult(1, 1, 1);
        }

        [Test]
        public void NormalBackpressured()
        {
            Flowable.Just(1)
                .RepeatWhen(f =>
                {
                    int[] counter = { 0 };

                    return f.TakeWhile(o => ++counter[0] < 5);
                })
                .Test(0)
                .AssertEmpty()
                .RequestMore(1)
                .AssertValues(1)
                .RequestMore(1)
                .AssertValues(1, 1)
                .RequestMore(1)
                .AssertValues(1, 1, 1)
                .RequestMore(1)
                .AssertValues(1, 1, 1, 1)
                .RequestMore(1)
                .AssertResult(1, 1, 1, 1, 1)
                ;
        }
    }
}
