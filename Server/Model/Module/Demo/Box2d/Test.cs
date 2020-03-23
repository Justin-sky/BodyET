using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Joints;
using NETCoreTest.Framework;

namespace ETModel
{
    public class Test: IContactListener
    {
        public static Test Instance { get; set; }

        public World World;

        private const int Count = 800;

        public int FrameCount = 2000;

        private RevoluteJoint _joint;

        private int _bodyCount;

        public Profile MaxProfile = new Profile();

        public Profile TotalProfile = new Profile();

        public FixedUpdate FixedUpdate;

        public FpsCounter FpsCounter = new FpsCounter();

        public Test()
        {
            Instance = this;
            World = new World(new Vector2(0, 0));
            World.AllowSleep = false;
            World.SetContactListener(this);
        }

        public void Tumbler(bool stressTest = false)
        {
            Body ground;
            {
                var bd = new BodyDef();
                ground = World.CreateBody(bd);
            }

            //{
            //    var bd = new BodyDef
            //    {
            //        BodyType = BodyType.DynamicBody,
            //        AllowSleep = false,
            //        Position = new Vector2(0.0f, 10.0f)
            //    };
            //    var body = World.CreateBody(bd);

            //    var shape = new PolygonShape();
            //    shape.SetAsBox(0.5f, 10.0f, new Vector2(10.0f, 0.0f), 0.0f);
            //    body.CreateFixture(shape, 5.0f);
            //    shape.SetAsBox(0.5f, 10.0f, new Vector2(-10.0f, 0.0f), 0.0f);
            //    body.CreateFixture(shape, 5.0f);
            //    shape.SetAsBox(10.0f, 0.5f, new Vector2(0.0f, 10.0f), 0.0f);
            //    body.CreateFixture(shape, 5.0f);
            //    shape.SetAsBox(10.0f, 0.5f, new Vector2(0.0f, -10.0f), 0.0f);
            //    body.CreateFixture(shape, 5.0f);

            //    var jd = new RevoluteJointDef
            //    {
            //        BodyA = ground,
            //        BodyB = body,
            //        LocalAnchorA = new Vector2(0.0f, 10.0f),
            //        LocalAnchorB = new Vector2(0.0f, 0.0f),
            //        ReferenceAngle = 0.0f,
            //        MotorSpeed = 0.05f * Settings.Pi,
            //        MaxMotorTorque = 1e8f,
            //        EnableMotor = true
            //    };
            //    _joint = (RevoluteJoint)World.CreateJoint(jd);
            //}

            //_bodyCount = 0;
            //if (stressTest)
            //{
            //    var timer = Stopwatch.StartNew();
            //    for (int i = 0; i < FrameCount; i++)
            //    {
            //        Step(false);
            //    }

            //    timer.Stop();
            //    Console.WriteLine($"{timer.ElapsedMilliseconds} ms");
            //}
            //else
            {
                FixedUpdate = new FixedUpdate(TimeSpan.FromSeconds(1 / 60d), () => { Step(true); });
                FixedUpdate.Start();
                //while (true)
                //{
                //    FixedUpdate.Update();
                //}
            }
        }

        private Dictionary<Body, Body2dComponent> bodyComponents { get; set; } = new Dictionary<Body, Body2dComponent>();
        public Body CreateBoxCollider(Body2dComponent component, float x, float y, float hx, float hy)
        {
            var bd = new BodyDef
            {
                BodyType = BodyType.DynamicBody,
                Position = new Vector2(x, y)
            };
            var body = World.CreateBody(bd);
            body.IsBullet = false;
            var shape = new PolygonShape();
            shape.SetAsBox(hx, hy, Vector2.Zero, 0);
            body.CreateFixture(shape, 1.0f);
            body.UserData = component;

            bodyComponents.Add(body, component);
            return body;
        }

        public void Remove(Body body)
        {
            if (bodyComponents.ContainsKey(body))
            {
                bodyComponents.Remove(body);
            }
            World.DestroyBody(body);
        }

        private float _dt = 1 / 60f;

        private readonly StringBuilder _sb = new StringBuilder();

        private void Step(bool showProfile)
        {
            World.Step(_dt, 8, 3);
            FpsCounter.CountOne();
            //if (_bodyCount < Count)
            //{
            //    var bd = new BodyDef
            //    {
            //        BodyType = BodyType.DynamicBody,
            //        Position = new Vector2(0.0f, 10.0f)
            //    };
            //    var body = World.CreateBody(bd);

            //    var shape = new PolygonShape();
            //    shape.SetAsBox(0.125f, 0.125f);
            //    body.CreateFixture(shape, 1.0f);

            //    ++_bodyCount;
            //}

            if (showProfile)
            {
                var p = World.Profile;

                // Track maximum profile times
                MaxProfile.Step = Math.Max(MaxProfile.Step, p.Step);
                MaxProfile.Collide = Math.Max(MaxProfile.Collide, p.Collide);
                MaxProfile.Solve = Math.Max(MaxProfile.Solve, p.Solve);
                MaxProfile.SolveInit = Math.Max(MaxProfile.SolveInit, p.SolveInit);
                MaxProfile.SolveVelocity = Math.Max(MaxProfile.SolveVelocity, p.SolveVelocity);
                MaxProfile.SolvePosition = Math.Max(MaxProfile.SolvePosition, p.SolvePosition);
                MaxProfile.SolveTOI = Math.Max(MaxProfile.SolveTOI, p.SolveTOI);
                MaxProfile.Broadphase = Math.Max(MaxProfile.Broadphase, p.Broadphase);

                TotalProfile.Step += p.Step;
                TotalProfile.Collide += p.Collide;
                TotalProfile.Solve += p.Solve;
                TotalProfile.SolveInit += p.SolveInit;
                TotalProfile.SolveVelocity += p.SolveVelocity;
                TotalProfile.SolvePosition += p.SolvePosition;
                TotalProfile.SolveTOI += p.SolveTOI;
                TotalProfile.Broadphase += p.Broadphase;

                var aveProfile = new Profile();
                if (FixedUpdate.TickCount > 0)
                {
                    var scale = 1.0f / FixedUpdate.TickCount;
                    aveProfile.Step = scale * TotalProfile.Step;
                    aveProfile.Collide = scale * TotalProfile.Collide;
                    aveProfile.Solve = scale * TotalProfile.Solve;
                    aveProfile.SolveInit = scale * TotalProfile.SolveInit;
                    aveProfile.SolveVelocity = scale * TotalProfile.SolveVelocity;
                    aveProfile.SolvePosition = scale * TotalProfile.SolvePosition;
                    aveProfile.SolveTOI = scale * TotalProfile.SolveTOI;
                    aveProfile.Broadphase = scale * TotalProfile.Broadphase;
                }

                //_sb.AppendLine($"FPS {FpsCounter.Fps}, ms {FpsCounter.Ms}");
                //_sb.AppendLine($"step [ave] (max) = {p.Step} [{aveProfile.Step}] ({MaxProfile.Step})");
                //_sb.AppendLine($"collide [ave] (max) = {p.Collide} [{aveProfile.Collide}] ({MaxProfile.Collide})");
                //_sb.AppendLine($"solve [ave] (max) = {p.Solve} [{aveProfile.Solve}] ({MaxProfile.Solve})");
                //_sb.AppendLine($"solve init [ave] (max) = {p.SolveInit} [{aveProfile.SolveInit}] ({MaxProfile.SolveInit})");
                //_sb.AppendLine($"solve velocity [ave] (max) = {p.SolveVelocity} [{aveProfile.SolveVelocity}] ({MaxProfile.SolveVelocity})");
                //_sb.AppendLine($"solve position [ave] (max) = {p.SolvePosition} [{aveProfile.SolvePosition}] ({MaxProfile.SolvePosition})");
                //_sb.AppendLine($"solveTOI [ave] (max) = {p.SolveTOI} [{aveProfile.SolveTOI}] ({MaxProfile.SolveTOI})");
                //_sb.AppendLine($"broad-phase [ave] (max) = {p.Broadphase} [{aveProfile.Broadphase}] ({MaxProfile.Broadphase})");

                //Console.SetCursorPosition(0, 0);
                //Console.Write(_sb.ToString());
                //_sb.Clear();
            }
        }

        public void BeginContact(Contact contact)
        {
            if (bodyComponents.ContainsKey(contact.FixtureA.Body) && bodyComponents.ContainsKey(contact.FixtureB.Body))
            {
                var bodyA = bodyComponents[contact.FixtureA.Body];
                var bodyB = bodyComponents[contact.FixtureB.Body];
                if (bodyA.Parent?.GetType() != bodyB.Parent?.GetType())
                {
                    bodyA.BeginContact(contact, bodyB);
                    bodyB.BeginContact(contact, bodyA);
                }
            }
        }

        public void EndContact(Contact contact)
        {
            if (bodyComponents.ContainsKey(contact.FixtureA.Body))
            {
                bodyComponents[contact.FixtureA.Body].EndContact(contact);
            }
        }

        public void PostSolve(Contact contact, in ContactImpulse impulse)
        {

        }

        public void PreSolve(Contact contact, in Manifold oldManifold)
        {

        }
    }
}