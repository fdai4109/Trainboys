using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PinballProjekt
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        //Matrix worldMatrix;

        Vector3 _platteLocation = new Vector3(0f, 0f, 0f);
        Vector3 _pinballLocation = new Vector3(0f, 0f, 0f);
        Vector3 _triggerRLocation = new Vector3(-10f, -1f, -40f);
        Vector3 _triggerLLocation = new Vector3(10f, -1f, -40f);
        Vector3 _bumperLocation = new Vector3(0f, 0f, 20f);
        Vector3 _bumper2Location = new Vector3(8f, 0f, 25f);
        Vector3 _bumper3Location = new Vector3(-8f, 0f, 25f);
        Vector3 _bumper4Location = new Vector3(0f, 0f, 30f);
        Vector3 _sideBumperLLocation = new Vector3(15f, -1f, -10f);
        Vector3 _sideBumperRLocation = new Vector3(-15f, -1f, -10f);

        Rectangle BoundingBox;

        Model _pinball;
        Model _platte;
        Model _triggerR;
        Model _triggerL;
        Model _bumper;
        Model _bumper2;
        Model _bumper3;
        Model _bumper4;
        Model _sideBumperR;
        Model _sideBumperL;

        Vector3 _collisionPosition;
        Vector3 _pinballLocationOLD = new Vector3(0f, 0f, 0f);
        Vector3 _triggerLPressed = new Vector3(10f, -1f, -30f);
        Vector3 _triggerLNormal = new Vector3(10f, -1f, -40f);
        Vector3 _triggerRPressed = new Vector3(-10f, -1f, -30f);
        Vector3 _triggerRNormal = new Vector3(-10f, -1f, -40f);
        //float _streckeX;
        //float _streckeY;

        float _timer = 0f;
        float _velocityX = -0.3f;
        float _velocityZ = -0.5f;

        //Orbit oder nicht Orbit?
        bool orbit;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        
        

        protected override void Initialize()
        {
            base.Initialize();

            //Kamera-Setup
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 100f, -180f); //Anfangsposition: Schräge Sicht
            //camPosition = new Vector3(0f, 150f, -1f); //Draufsicht
            //camPosition = new Vector3(0f, 0.5f, 10f); //Bumperansicht

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
            //worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);            

            _pinball = Content.Load<Model>("Pinball");
            _platte = Content.Load<Model>("Platte");
            _triggerR = Content.Load<Model>("Trigger");
            _triggerL = Content.Load<Model>("Trigger");
            _bumper = Content.Load<Model>("Bumper");
            _bumper2 = Content.Load<Model>("Bumper");
            _bumper3 = Content.Load<Model>("Bumper");
            _bumper4 = Content.Load<Model>("Bumper");
            _sideBumperR = Content.Load<Model>("SideBumper");
            _sideBumperL = Content.Load<Model>("SideBumper");
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //Game mit ESC schließen
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Matrix _pinballMatrix = Matrix.CreateTranslation(_pinballLocation);
            Matrix _platteMatrix = Matrix.CreateTranslation(_platteLocation);
            Matrix _triggerRMatrix = Matrix.CreateTranslation(_triggerRLocation);
            Matrix _triggerLMatrix = Matrix.CreateTranslation(_triggerLLocation);
            Matrix _bumperMatrix = Matrix.CreateTranslation(_bumperLocation);
            Matrix _bumper2Matrix = Matrix.CreateTranslation(_bumper2Location);
            Matrix _bumper3Matrix = Matrix.CreateTranslation(_bumper3Location);
            Matrix _bumper4Matrix = Matrix.CreateTranslation(_bumper4Location);
            Matrix _sideBumperLMatrix = Matrix.CreateTranslation(_sideBumperLLocation);
            Matrix _sideBumperRMatrix = Matrix.CreateTranslation(_sideBumperRLocation);

            /* _streckeX = _pinballLocationOLD.X - _pinballLocation.X;
             _streckeY = _pinballLocationOLD.Y - _pinballLocation.Y;

             _velocityX = _streckeX / _timer;
             _velocityY = _streckeY / _timer;*/

            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_timer >= 200f)
            {
                _pinballLocationOLD = _pinballLocation;
                _velocityZ -= 0.07f;
                _timer = 0f;
            }

            if(EdgeCollisionObenUnten())
            {
                //_pinballLocation += einfall(_pinballLocationOLD, _collisionPosition);
                //_velocityX *= 1f;
                _velocityZ *= -1f;
                System.Diagnostics.Debug.WriteLine("Collision");
            }

            if(EdgeCollisionRechtsLinks())
            {
                _velocityX *= -1;
                //_velocityZ *= 1;
            }

            if(Rtrigger())
            {
                _velocityX *= 1f;
                _velocityZ *= -1f;
                System.Diagnostics.Debug.WriteLine("KEINE COLLISION");
            }

            if(Ltrigger())
            {
                _velocityX *= 1f;
                _velocityZ *= -1f;
                System.Diagnostics.Debug.WriteLine("KEINE COLLISION");
            }

            if(LsideBumper())
            {
                _velocityX *= -1;
            }

            if(RsideBumper())
            {
                _velocityX *= -1;
            }

            if (Bumper())
            {
                _velocityX *= 1;
                _velocityZ *= -1;
                System.Diagnostics.Debug.WriteLine(_pinballLocation);
            }

            if (Bumper2())
            {
                _velocityX *= 1;
                _velocityZ *= -1;
                System.Diagnostics.Debug.WriteLine(_pinballLocation);
            }

            if (Bumper3())
            {
                _velocityX *= 1;
                _velocityZ *= -1;
                System.Diagnostics.Debug.WriteLine(_pinballLocation);
            }

            if (Bumper4())
            {
                _velocityX *= 1;
                _velocityZ *= -1;
                System.Diagnostics.Debug.WriteLine(_pinballLocation);
            }

            /*if (IsCollision(_pinball, _pinballMatrix, _platte, _platteMatrix))
            {
                _pinballLocation += einfall(_pinballLocationOLD, _collisionPosition);
                _velocityX *= 1;
                _velocityY *= -1;
                System.Diagnostics.Debug.WriteLine("ISCOLLI");
            }*/

            //Kugel-Bewegung
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _pinballLocation.X += 1f;
                //camTarget.X -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _pinballLocation.X -= 1f;
                //camTarget.X += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _pinballLocation.Y += 1f;
                //camTarget.Y -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _pinballLocation.Y -= 1f;
                //camTarget.Y += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                _triggerLLocation = _triggerLPressed;
                System.Diagnostics.Debug.WriteLine("GEDRÜCKT");
            }
            if(Keyboard.GetState().IsKeyUp(Keys.K))
            {
                _triggerLLocation = _triggerLNormal;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.L))
            {
                _triggerRLocation = _triggerRPressed;
            }
            
            if(Keyboard.GetState().IsKeyUp(Keys.L))
            {
                _triggerRLocation = _triggerRNormal;
            }
            
            //Kamera dreht sich automatisch um Target, Orbit
            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            System.Diagnostics.Debug.WriteLine(_pinballLocation.Y + " - Aktuell Y");
            System.Diagnostics.Debug.WriteLine(_pinballLocationOLD.Y + " - Alt Y");
            System.Diagnostics.Debug.WriteLine(_pinballLocation.X + " - Aktuell X");
            System.Diagnostics.Debug.WriteLine(_pinballLocationOLD.X + " - Alt X");
            System.Diagnostics.Debug.WriteLine(_pinballLocation.Z + " - AKtuell Z");
            System.Diagnostics.Debug.WriteLine(_pinballLocationOLD.Z + "- Alt Z ");

            System.Diagnostics.Debug.WriteLine(_velocityX + "- Vel X");
            System.Diagnostics.Debug.WriteLine(_velocityZ + "- Vel Y");

            _pinballLocation.Z += _velocityZ;
            _pinballLocation.X += _velocityX;

            base.Update(gameTime);
        }

        private Vector3 einfall(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z)*5;
        }

        private bool IsCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                    {
                        _collisionPosition = _pinballLocation;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool Rtrigger()
        {
            if(_pinballLocation.Z <= -39f && _pinballLocation.Z >=-41f)
            {
                if(_pinballLocation.X <= -5f && _pinballLocation.X >= -15f)
                {
                    return true;
                }
                
            }
            return false;
        }

        private bool Ltrigger()
        {
            if (_pinballLocation.Z <= -39f && _pinballLocation.Z >= -41f)
            {
                if (_pinballLocation.X >= 5f && _pinballLocation.X <= 15f)
                {
                    return true;
                }

            }
            return false;
        }

        private bool LsideBumper()
        {
            if(_pinballLocation.Z <=-6 && _pinballLocation.Z >= -14)
            {
                if(_pinballLocation.X >= 13 && _pinballLocation.X <=17)
                {
                    return true;
                }
            }
            return false;
        }

        private bool RsideBumper()
        {
            if (_pinballLocation.Z <= -6 && _pinballLocation.Z >= -14)
            {
                if (_pinballLocation.X >= -17 && _pinballLocation.X <= -13)
                {
                    return true;
                }
            }
            return false;
        }

        private bool EdgeCollisionObenUnten()
        {
            
             if(_pinballLocation.Z >= 48)
            {
                _collisionPosition = _pinballLocation;
                return true;
            }
            else if(_pinballLocation.Z <= -48)
            {
                _collisionPosition = _pinballLocation;
                return true;
            }
            
            
                return false;
        }

        private bool Bumper()
        {
            if (_pinballLocation.Z <= 22f && _pinballLocation.Z >= 18f)
            {
                if (_pinballLocation.X <= 2f && _pinballLocation.X >= -2f)
                {
                    //BumperReaktion();
                    return true;
                }

            }
            return false;
        }

        private bool Bumper2()
        {
            if (_pinballLocation.Z <= 27f && _pinballLocation.Z >= 23f)
            {
                if (_pinballLocation.X <= 10f && _pinballLocation.X >= 6f)
                {
                    //BumperReaktion();
                    return true;
                }
            }
            return false;
        }

        private bool Bumper3()
        {
            if (_pinballLocation.Z <= 27f && _pinballLocation.Z >= 23f)
            {
                if (_pinballLocation.X <= -10f && _pinballLocation.X >= -6f)
                {
                    //BumperReaktion();
                    return true;
                }
            }
            return false;
        }

        private bool Bumper4()
        {
            if (_pinballLocation.Z <= 32f && _pinballLocation.Z >= 28f)
            {
                if (_pinballLocation.X <= 2f && _pinballLocation.X >= -2f)
                {
                    //BumperReaktion();
                    return true;
                }
            }
            return false;
        }

        private bool EdgeCollisionRechtsLinks()
        {
            if (_pinballLocation.X >= 23)
            {
                _collisionPosition = _pinballLocation;
                return true;
            }
            else if (_pinballLocation.X <= -23)
            {
                _collisionPosition = _pinballLocation;
                return true;
            }
            return false;
        }

        public Rectangle Box(int x, int y, int breite, int höhe)
        {
            Rectangle collision = new Rectangle(x, y, breite, höhe);
            return collision;
        }

        public void BumperReaktion()
        {
            /*do
            {
                if (_bumperLocation.Y >= -1f)
                {
                    _bumperLocation.Y -= 0.1f;
                }
                else if (_bumperLocation.Y <= 0f)
                {
                    _bumperLocation.Y += 0.1f;
                }
                
            } while (_bumperLocation.Y < 0f);*/
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix _platteMatrix = Matrix.CreateTranslation(_platteLocation);
            Matrix _pinballMatrix = Matrix.CreateTranslation(_pinballLocation);
            Matrix _triggerRMatrix = Matrix.CreateTranslation(_triggerRLocation);
            Matrix _triggerLMatrix = Matrix.CreateTranslation(_triggerLLocation);
            Matrix _bumperMatrix = Matrix.CreateTranslation(_bumperLocation);
            Matrix _bumper2Matrix = Matrix.CreateTranslation(_bumper2Location);
            Matrix _bumper3Matrix = Matrix.CreateTranslation(_bumper3Location);
            Matrix _bumper4Matrix = Matrix.CreateTranslation(_bumper4Location);
            Matrix _sideBumperLMatrix = Matrix.CreateTranslation(_sideBumperLLocation);
            Matrix _sideBumperRMatrix = Matrix.CreateTranslation(_sideBumperRLocation);

            DrawModel(_platte, _platteMatrix, viewMatrix, projectionMatrix);
            DrawModel(_pinball, _pinballMatrix, viewMatrix, projectionMatrix);
            DrawModel(_triggerR, _triggerRMatrix, viewMatrix, projectionMatrix);
            DrawModel(_triggerL, _triggerLMatrix, viewMatrix, projectionMatrix);
            DrawModel(_bumper, _bumperMatrix, viewMatrix, projectionMatrix);
            DrawModel(_bumper2, _bumper2Matrix, viewMatrix, projectionMatrix);
            DrawModel(_bumper3, _bumper3Matrix, viewMatrix, projectionMatrix);
            DrawModel(_bumper4, _bumper4Matrix, viewMatrix, projectionMatrix);
            DrawModel(_sideBumperL, _sideBumperLMatrix, viewMatrix, projectionMatrix);
            DrawModel(_sideBumperR, _sideBumperRMatrix, viewMatrix, projectionMatrix);
            /*foreach (ModelMesh mesh in _pinball.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

            foreach (ModelMesh mesh in _platte.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }*/

            base.Draw(gameTime);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = view;
                    effect.World = world;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}
