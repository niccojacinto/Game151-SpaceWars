#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace SpaceWars
{
    public class Particle
    {
        private Vector4 color;
        private Vector4 startColor;
        private Vector4 endColor;
        private TimeSpan endTime = TimeSpan.Zero;
        private TimeSpan lifetime;
        public Vector3 position;
        Vector3 velocity;
        protected Vector3 acceleration = new Vector3( 1.0f, 1.0f, 1.0f );

        public bool Delete;
        
        public Vector4 Color
        {
            get
            {
                 return color;
            }
        }
      
        public Particle(Vector2 position2, Vector2 velocity2, Vector4 startColor, Vector4 endColor, TimeSpan lifetime)
        {
            velocity = new Vector3(velocity2, 0.0f);
            position = new Vector3(position2, 0.0f);
            this.startColor = startColor;
            this.endColor = endColor;
            this.lifetime = lifetime;
        }

        public void Update(TimeSpan time, TimeSpan elapsedTime)
        {
            if (endTime == TimeSpan.Zero)
            {
                endTime = time + lifetime;
            }

            if (time > endTime)
            {
                Delete = true;
            }

            float percentLife = (float)((endTime.TotalSeconds - time.TotalSeconds) / lifetime.TotalSeconds);

            color = Vector4.Lerp(endColor, startColor, percentLife);
           
            velocity += Vector3.Multiply(acceleration, (float)elapsedTime.TotalSeconds);
            position += Vector3.Multiply(velocity, (float)elapsedTime.TotalSeconds);

        }
    }


    public class BasicParticleSystem
    {
        private static Random random = new Random();

        List<Particle> particleList = new List<Particle>();

        Texture2D circle;
        int Count = 0;

        public BasicParticleSystem(Texture2D circle)
        {
            this.circle = circle;
        }

        public void AddExplosion(Vector2 position)
        {
            for (int i = 0; i < 300; i++)
            {
                Vector2 velocity2 = (float)random.Next(100) * Vector2.Normalize(new Vector2((float)(random.NextDouble() - .5), (float)(random.NextDouble() - .5)));
                particleList.Add(new Particle(
                        position,
                        velocity2,
                        new Vector4(1,1,1,1),
                        new Vector4(.2f, .2f, .2f, 0f),
                        new TimeSpan(0, 0, 0, 0, random.Next(1000) + 500)));
                Count++;
            }
        }

        public void AddExplosion2 ( Vector2 position ) {
            for ( int i = 0; i < 20; i++ ) {
                Vector2 velocity2 = (float)random.Next ( 300 ) * Vector2.Normalize ( new Vector2 ( (float)( random.NextDouble () - .5 ), (float)( random.NextDouble () - .5 ) ) );
                particleList.Add ( new Particle (
                        position,
                        velocity2,
                        new Vector4 ( .54f, .27f, 0.07f, 1 ),
                        new Vector4 ( .54f, .27f, 0.07f, 0f ),
                        new TimeSpan ( 0, 0, 0, 0, random.Next ( 1000 ) + 500 ) ) );
                Count++;
            }
        }

        public void AddExplosion3 ( Vector2 position ) {
            for ( int i = 0; i < 1000; i++ ) {
                Vector2 velocity2 = (float)random.Next ( 100 ) * Vector2.Normalize ( new Vector2 ( (float)( random.NextDouble () - .5 ), (float)( random.NextDouble () - .5 ) ) );
                particleList.Add ( new Particle (
                        position,
                        velocity2,
                        new Vector4 ( .0f, .0f, 0.9f, 1f ),
                        new Vector4 ( .0f, .0f, 0.9f, 0f ),
                        new TimeSpan ( 0, 0, 0, 0, random.Next ( 1000 ) + 500 ) ) );
                Count++;
            }
        }


        public void Update(TimeSpan time, TimeSpan elapsed)
        {
            if (Count > 0)
            {
                for( int i = 0; i < particleList.Count; i++ )
                {
                    particleList[i].Update(time, elapsed);
                    if (particleList[i].Delete) particleList.RemoveAt(i);
                }
                Count = particleList.Count;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (Count != 0)
            {

              //  int particleCount = 0;

                foreach (Particle particle in particleList)
                {
                    batch.Draw(circle,
                        new Vector2(particle.position.X, particle.position.Y),
                        null, new Color(((Particle)particle).Color), 0,
                        new Vector2(16, 16), .3f,
                        SpriteEffects.None, particle.position.Z);
                 //   particleCount++;
                }
            }
        }
    }
}
