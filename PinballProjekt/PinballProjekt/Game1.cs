using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PinballProjekt
{
    public class Game1 : Game
    {
        //GIT IST MIST!
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont font;
        private int score = 0;

        double distance;
        double Xdis;
        double Zdis;
        String Colli;


        Matrix projectionMatrix;
        Matrix viewMatrix;
        //Matrix worldMatrix;

        #region Start Locations
        Vector3 camTarget;
        Vector3 camPosition;
        Vector3 _platteLocation = new Vector3(0f, 0f, 0f);
        Vector3 _pinballLocation = new Vector3(0f, 0f, 35f);
        //Vector3 _pinballLocation = new Vector3(-23f, 0f, -48f);
        //Vector3 _pinballLocation = new Vector3(0f, 0f, 0f);
        Vector3 _triggerRLocation = new Vector3(-10f, -1f, -40f);
        Vector3 _triggerLLocation = new Vector3(10f, -1f, -40f);
        Vector3 _bumperLocation = new Vector3(0f, 0f, 20f);
        Vector3 _bumper2Location = new Vector3(8f, 0f, 25f);
        Vector3 _bumper3Location = new Vector3(-8f, 0f, 25f);
        Vector3 _bumper4Location = new Vector3(0f, 0f, 30f);
        Vector3 _sideBumperLLocation = new Vector3(15f, -1f, -10f);
        Vector3 _sideBumperRLocation = new Vector3(-15f, -1f, -10f);
        Vector3 _startRampeLocation = new Vector3(0f, -1f, -1f);
        Vector3 _grenzeRechtsLocation = new Vector3(-18f, -1f, -44f);
        Vector3 _grenzeLinksLocation = new Vector3(+18f, -1f, -44f);
        Vector3 _movingBumper1Location = new Vector3(15f, -1f, 5f);
        Vector3 _movingBumper2Location = new Vector3(-15f, -1f, -25f);
        //Vector3 _bumperWand1Location = new Vector3(0f, -1f, 0);
        #endregion

        #region Models
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
        Model _startRampe;
        Model _grenzeRechts;
        Model _grenzeLinks;
        Model _movingBumper1;
        Model _movingBumper2;
        //Model _bumperWand1;
        #endregion

        #region Gespeicherte Locations
        Vector3 _collisionPosition;
        Vector3 _pinballLocationOLD = new Vector3(5f, 0f, 0f);
        Vector3 _triggerLPressed = new Vector3(10f, -1f, -30f);
        Vector3 _triggerLNormal = new Vector3(10f, -1f, -40f);
        //Vector3 _triggerRPressed = new Vector3(-10f, -1f, -30f);
        //Vector3 _triggerRNormal = new Vector3(-10f, -1f, -40f);
        #endregion

        #region Floats
        //float _streckeX;
        //float _streckeY;

        float _timer = 0f;
        float _velocityX = 0.3f;
        float _velocityZ = 1f;
        float _triggerRvelocityZ = 0.2f;
        float _triggerLvelocityZ = 0.2f;
        float _triggerRvelocityX = 0.2f;
        float _triggerLvelocityX = 0.2f;
        #endregion

        #region Bools
        bool orbit; //Orbit oder nicht Orbit?
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            #region Kamera-Setup
            camTarget = new Vector3(0f, 0f, 0f);
            //camPosition = new Vector3(0f, 100f, -80f); //Anfangsposition: Schräge Sicht
            camPosition = new Vector3(0f, 150f, -1f); //Draufsicht
            //camPosition = new Vector3(0f, 0.5f, 10f); //Bumperansicht

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
            //worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            #endregion

            #region Content laden
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
            _startRampe = Content.Load<Model>("Rampe");
            _grenzeRechts = Content.Load<Model>("GrenzeRechts");
            _grenzeLinks = Content.Load<Model>("GrenzeLinks");
            _movingBumper1 = Content.Load<Model>("Bumper");
            _movingBumper2 = Content.Load<Model>("Bumper");
            //_bumperWand1 = Content.Load<Model>("BumperWand");
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (gamePadState.Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit(); //Game mit ESC schließen
            }

            #region Matrizen werden platziert
            Matrix _pinballMatrix = Matrix.CreateTranslation(_pinballLocation);
            //Matrix _platteMatrix = Matrix.CreateTranslation(_platteLocation);
            Matrix _triggerRMatrix = Matrix.CreateTranslation(_triggerRLocation);
            Matrix _triggerLMatrix = Matrix.CreateTranslation(_triggerLLocation);
            //Matrix _bumperMatrix = Matrix.CreateTranslation(_bumperLocation);
            //Matrix _bumper2Matrix = Matrix.CreateTranslation(_bumper2Location);
            //Matrix _bumper3Matrix = Matrix.CreateTranslation(_bumper3Location);
            //Matrix _bumper4Matrix = Matrix.CreateTranslation(_bumper4Location);
            //Matrix _sideBumperLMatrix = Matrix.CreateTranslation(_sideBumperLLocation);
            //Matrix _sideBumperRMatrix = Matrix.CreateTranslation(_sideBumperRLocation);
            //Matrix _startRampeMatrix = Matrix.CreateTranslation(_startRampeLocation);
            //Matrix _grenzeRechtsMatrix = Matrix.CreateTranslation(_grenzeRechtsLocation);
            //Matrix _grenzeLinksMatrix = Matrix.CreateTranslation(_grenzeLinksLocation);
            //Matrix _bumperWand1Matrix = Matrix.CreateTranslation(_bumperWand1Location);
            #endregion

            /* _streckeX = _pinballLocationOLD.X - _pinballLocation.X;
             _streckeY = _pinballLocationOLD.Y - _pinballLocation.Y;

             _velocityX = _streckeX / _timer;
             _velocityY = _streckeY / _timer;*/

            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_timer >= 200f)
            {
                //_pinballLocationOLD = _pinballLocation;
                if (_velocityZ >= -1.5f)
                {
                    _velocityZ -= 0.07f;
                }
                _timer = 0f;
            }

            #region Geschwindigkeitsbegrenzung der Kugel
            if(_velocityZ >= 1.8f)
            {
                _velocityZ = 1.8f;
            }

            if (_velocityZ <= -1.8f)
            {
                _velocityZ = -1.8f;
            }

            if(_velocityX >= 1.8f)
            {
                _velocityX = 1.8f;
            }

            if(_velocityX <= -1.8f)
            {
                _velocityX = -1.8f;
            }
            #endregion

            #region Wandkollision
            if (EdgeCollisionObenUnten())
            {
                Colli = "null";
                //_pinballLocation += einfall(_pinballLocationOLD, _collisionPosition);
                //_velocityX *= 1f;
                _velocityZ *= -1f;
                //System.Diagnostics.Debug.WriteLine("Collision");
            }

            if (EdgeCollisionRechtsLinks())
            {
                Colli = "null";
                _velocityX *= -1;
                //_velocityZ *= 1;
            }
            #endregion

            #region Trigger-Abfragen
            if (Rtrigger())
            {
                Colli = "null";
                //_velocityX *= 1f;
                _velocityZ *= -1.2f;
                //System.Diagnostics.Debug.WriteLine("TriggerR-COLLISION");
            }

            if (Ltrigger())
            {
                Colli = "null";
                //_velocityX *= 1f;
                _velocityZ *= -1.2f;
                //System.Diagnostics.Debug.WriteLine("TriggerL-COLLISION");
            }
            #endregion

            #region Grenzen-Abfragen
            if (GrenzeRechts())
            {
                Colli = "null";
                _velocityZ *= -1.2f;
                /*System.Diagnostics.Debug.WriteLine("Grenzen-COLLISION");*/
            }

            if(GrenzeLinks())
            {
                Colli = "null";
                _velocityZ *= -1.2f;
                /*System.Diagnostics.Debug.WriteLine("Grenzen-COLLISION");*/
            }

            if(GrenzeMitte())
            {
                Colli = "null";
                _pinballLocation = _pinballLocationOLD;
                score = 0;
                _velocityX = 0.1f;
                _velocityZ = 0;
            }
            #endregion

            #region SideBumper-Abfragen
            if (LsideBumper())
            {
                if (Colli != "S1")
                {
                    Colli = "S1";
                    if (_pinballLocation.Z <= -5)
                    {
                        _velocityZ *= -1;
                    }
                    if (_pinballLocation.Z >= -15)
                    {
                        _velocityZ *= -1;
                    }
                    else
                    {
                        _velocityX *= -1;
                    }
                    score += 10;
                    System.Diagnostics.Debug.WriteLine("S2: " + _sideBumperLLocation + " zu " + _pinballLocation);
                }
            }

            if (RsideBumper())
            {
                if (Colli != "S2")
                {
                    Colli = "S2";
                    if (_pinballLocation.Z <= -5)
                    {
                        _velocityZ *= -1;
                    }
                    if (_pinballLocation.Z >= -15)
                    {
                        _velocityZ *= -1;
                    }
                    else
                    {
                        _velocityX *= -1;
                    }
                    score += 10;
                    System.Diagnostics.Debug.WriteLine("S2: " + _sideBumperRLocation + " zu " + _pinballLocation);
                }
            }
            #endregion
            
            #region Bumper-Abfragen
            if (Bumper())
            {
                if (Colli != "B1")
                {
                    Colli = "B1";
                    if (_pinballLocation.Z <= _bumperLocation.Z + 1 && _pinballLocation.Z >= _bumperLocation.Z - 1)
                    {
                        _velocityX *= -1;
                    }
                    else if (_pinballLocation.X <= _bumperLocation.X + 1 && _pinballLocation.X >= _bumperLocation.X - 1)
                    {
                        _velocityZ *= -1;
                    }
                    else
                    {
                        _velocityX *= -1;
                        _velocityZ *= -1;
                    }
                    System.Diagnostics.Debug.WriteLine("B1: " + _bumperLocation + " zu " + _pinballLocation);
                    score += 5;
                }
            }

            if (Bumper2())
            {
                if (Colli != "B2")
                {
                    Colli = "B2";
                    if (_pinballLocation.Z <= _bumper2Location.Z + 1 && _pinballLocation.Z >= _bumper2Location.Z - 1)
                    {
                        _velocityX *= -1;
                    }
                    else if (_pinballLocation.X <= _bumper2Location.X + 1 && _pinballLocation.X >= _bumper2Location.X - 1)
                    {
                        _velocityZ *= -1;
                    }
                    else
                    {
                        _velocityX *= -1;
                        _velocityZ *= -1;
                    }
                    System.Diagnostics.Debug.WriteLine("B2: " + _bumper2Location + " zu " + _pinballLocation);
                    score += 5;
                }
            }

            if (Bumper3())
            {
                if (Colli != "B3")
                {
                    Colli = "B3";
                    if (_pinballLocation.Z <= _bumper3Location.Z + 1 && _pinballLocation.Z >= _bumper3Location.Z - 1)
                    {
                        _velocityX *= -1;
                    }
                    else if (_pinballLocation.X <= _bumper3Location.X + 1 && _pinballLocation.X >= _bumper3Location.X - 1)
                    {
                        _velocityZ *= -1;
                    }
                    else
                    {
                        _velocityX *= -1;
                        _velocityZ *= -1;
                    }
                    System.Diagnostics.Debug.WriteLine("B3: " + _bumper3Location + " zu " + _pinballLocation);
                    score += 5;
                }
            }

            if (Bumper4())
            {
                if (Colli != "B4")
                {
                    Colli = "B4";
                    if (_pinballLocation.Z <= _bumper4Location.Z + 1 && _pinballLocation.Z >= _bumper4Location.Z - 1)
                    {
                        _velocityX *= -1;
                    }
                    else if (_pinballLocation.X <= _bumper4Location.X + 1 && _pinballLocation.X >= _bumper4Location.X - 1)
                    {
                        _velocityZ *= -1;
                    }
                    else
                    {
                        _velocityX *= -1;
                        _velocityZ *= -1;
                    }

                    score += 5;
                    System.Diagnostics.Debug.WriteLine("B4: " + _bumper4Location + " zu " + _pinballLocation);
                }
            }
            #endregion

            #region MovingBumper-Abfragen
            if (MovingBumper1())
            {
                if (Colli != "M1")
                {
                    Colli = "M1";
                    if (_pinballLocation.Z <= _movingBumper1Location.Z + 1 && _pinballLocation.Z >= _movingBumper1Location.Z - 1)
                    {
                        _velocityX *= -1.1f;
                    }
                    else if (_pinballLocation.X <= _movingBumper1Location.X + 1 && _pinballLocation.X >= _movingBumper1Location.X - 1)
                    {
                        _velocityZ *= -1.1f;
                    }
                    else
                    {
                        _velocityX *= -1.1f;
                        _velocityZ *= -1.1f;
                    }
                    System.Diagnostics.Debug.WriteLine("M1: " + _movingBumper1Location + " zu " + _pinballLocation);
                    score += 15;
                }
            }

            if (MovingBumper2())
            {
                if (Colli != "M2")
                {
                    Colli = "M2";

                    if (_pinballLocation.Z <= _movingBumper2Location.Z + 1 && _pinballLocation.Z >= _movingBumper2Location.Z - 1)
                    {
                        _velocityX *= -1.1f;
                    }
                    else if (_pinballLocation.X <= _movingBumper2Location.X + 1 && _pinballLocation.X >= _movingBumper2Location.X - 1)
                    {
                        _velocityZ *= -1.1f;
                    }
                    else
                    {
                        _velocityX *= -1.1f;
                        _velocityZ *= -1.1f;
                    }
                    System.Diagnostics.Debug.WriteLine("M2: " + _movingBumper2Location + " zu " + _pinballLocation);
                    score += 15;
                }
            }
            #endregion

            #region Startrampe
            /*if (StartRampeUnten())
            {
                _velocityZ = 0.3f;
            }

            if(StartRampeOben())
            {
                _velocityZ = 0f;
                _velocityX = 0.3f;
            }

            if(StartRampeLinks())
            {
                _velocityX = -0.01f;
                _velocityZ = -0.1f;
            }*/
            #endregion

            #region Mauern Startrampe
            if (rechteWand())
            {
                _velocityX *= -1;
            }

            if(obereWand())
            {
                _velocityZ *= -1;
            }

            if(linkeWand())
            {
                _velocityX *= -1;
            }
            #endregion

            /*if (IsCollision(_pinball, _pinballMatrix, _platte, _platteMatrix))
            {
                _pinballLocation += einfall(_pinballLocationOLD, _collisionPosition);
                _velocityX *= 1;
                _velocityY *= -1;
                System.Diagnostics.Debug.WriteLine("ISCOLLI");
            }*/

            #region Kugelbewegung mit WASD
            /*if (Keyboard.GetState().IsKeyDown(Keys.A))
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
            }*/
            #endregion

            #region Kamerabewegung
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

            //Kamera dreht sich automatisch um Target, Orbit
            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
            #endregion

            //Eine Steuerung für sich auswählen und andere komplett auskommentieren.
            #region Triggerbewegung - Tastatur
            /*            
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                if (_triggerLLocation.Z <= -30f)
                {
                    _triggerLLocation.Z += _triggerLvelocityZ;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.K))
            {
                if (_triggerLLocation.Z >= -40f)
                {
                    _triggerLLocation.Z -= _triggerLvelocityZ;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {                
                if (_triggerRLocation.Z <= -30f)
                {
                    _triggerRLocation.Z += _triggerRvelocityZ;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.L))
            {
                if (_triggerRLocation.Z >= -40f)
                {
                    _triggerRLocation.Z -= _triggerRvelocityZ;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                if (_triggerRLocation.X <= -5f)
                {
                    _triggerRLocation.X += _triggerRvelocityX;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.O))
            {
                if (_triggerRLocation.X >= -10)
                {
                    _triggerRLocation.X -= _triggerRvelocityX;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                if (_triggerLLocation.X >= 5f)
                {
                    _triggerLLocation.X -= _triggerLvelocityX;                    
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.I))
            {
                if (_triggerLLocation.X <= 10f)
                {
                    _triggerLLocation.X += _triggerLvelocityX;
                }
            }
            */
            #endregion

            #region Triggerbewegung - Controller
            //Beide Trigger lassen sich mit dem jeweiligen Thumbstick des Xbox-Controllers steuern.
            //Funktioniert nur, wenn Tastatursteuerung auskommentiert ist.

            if (gamePadState.ThumbSticks.Left.Y >= 0.1f)
            {
                if (_triggerLLocation.Z <= -30f)
                {
                    _triggerLLocation.Z += _triggerLvelocityZ;
                }
            }

            if (gamePadState.ThumbSticks.Left.Y <= 0f)
            {
                if (_triggerLLocation.Z >= -40f)
                {
                    _triggerLLocation.Z -= _triggerLvelocityZ;
                }
            }

            if (gamePadState.ThumbSticks.Right.Y >= 0.1f)
            {
                if (_triggerRLocation.Z <= -30f)
                {
                    _triggerRLocation.Z += _triggerRvelocityZ;
                }
            }

            if (gamePadState.ThumbSticks.Right.Y <= 0f)
            {
                if (_triggerRLocation.Z >= -40f)
                {
                    _triggerRLocation.Z -= _triggerRvelocityZ;
                }
            }

            if (gamePadState.ThumbSticks.Right.X <= -0.1f)
            {
                if (_triggerRLocation.X <= -5f)
                {
                    _triggerRLocation.X += _triggerRvelocityX;
                }
            }

            if (gamePadState.ThumbSticks.Right.X >= 0f)
            {
                if (_triggerRLocation.X >= -10)
                {
                    _triggerRLocation.X -= _triggerRvelocityX;
                }
            }

            if (gamePadState.ThumbSticks.Left.X >= 0.1f)
            {
                if (_triggerLLocation.X >= 5f)
                {
                    _triggerLLocation.X -= _triggerLvelocityX;
                }
            }

            if (gamePadState.ThumbSticks.Left.X <= 0f)
            {
                if (_triggerLLocation.X <= 10f)
                {
                    _triggerLLocation.X += _triggerLvelocityX;
                }
            }

            //Knopfdruck beschleunigt Ball -- Nur ein Test!    
            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                _velocityZ *= 1.02f;
            }
            #endregion

            /*System.Diagnostics.Debug.WriteLine(_pinballLocation.Y + " - Aktuell Y");
            System.Diagnostics.Debug.WriteLine(_pinballLocationOLD.Y + " - Alt Y");
            System.Diagnostics.Debug.WriteLine(_pinballLocation.X + " - Aktuell X");
            System.Diagnostics.Debug.WriteLine(_pinballLocationOLD.X + " - Alt X");
            System.Diagnostics.Debug.WriteLine(_pinballLocation.Z + " - AKtuell Z");
            System.Diagnostics.Debug.WriteLine(_pinballLocationOLD.Z + "- Alt Z ");

            System.Diagnostics.Debug.WriteLine(_velocityX + "- Vel X");
            System.Diagnostics.Debug.WriteLine(_velocityZ + "- Vel Y");*/

            /*System.Diagnostics.Debug.WriteLine(_velocityZ);*/

            moveMovingBumper1();
            moveMovingBumper2();

            _pinballLocation.Z += _velocityZ;
            _pinballLocation.X += _velocityX;

            base.Update(gameTime);
        }

        /*private Vector3 einfall(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z)*5;
        }*/

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

        #region Trigger-Bool-Funktionen
        private bool Rtrigger()
        {
            if (_pinballLocation.Z <= _triggerRLocation.Z + 1 && _pinballLocation.Z >= _triggerRLocation.Z - 1 && _pinballLocation.X <= _triggerRLocation.X + 5f && _pinballLocation.X >= _triggerRLocation.X - 5)
            {
                return true;
            }
            return false;
        }

        private bool Ltrigger()
        {
            if (_pinballLocation.Z <= _triggerLLocation.Z + 1 && _pinballLocation.Z >= _triggerLLocation.Z - 1 && _pinballLocation.X >= _triggerLLocation.X - 5f && _pinballLocation.X <= _triggerLLocation.X + 5)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Sidebumper-Bool-Funktionen
        private bool LsideBumper()
        {
            if (_pinballLocation.Z <= -6 && _pinballLocation.Z >= -14 && _pinballLocation.X >= 13 && _pinballLocation.X <= 17)
            {
                return true;
            }
            return false;
        }

        private bool RsideBumper()
        {
            if (_pinballLocation.Z <= -6 && _pinballLocation.Z >= -14 && _pinballLocation.X >= -17 && _pinballLocation.X <= -13)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Wände-Bool-Funktionen
        private bool EdgeCollisionObenUnten()
        {
            if (_pinballLocation.Z >= 48)
            {
                _collisionPosition = _pinballLocation;
                return true;
            }
            else if (_pinballLocation.Z <= -48)
            {
                _collisionPosition = _pinballLocation;
                return true;
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
        #endregion

        #region Bumper-Bool-Funktionen-Distance
        private bool Bumper()
        {
            if (_pinballLocation.X >= _bumperLocation.X)
            {
                Xdis = _pinballLocation.X - _bumperLocation.X;
            }
            else
            {
                Xdis = _bumperLocation.X - _pinballLocation.X;
            }

            if (_pinballLocation.Z >= _bumperLocation.Z)
            {
                Zdis = _pinballLocation.Z - _bumperLocation.Z;
            }
            else
            {
                Zdis = _bumperLocation.Z - _pinballLocation.Z;
            }

            distance = Math.Sqrt(Math.Pow(Xdis, 2) + Math.Pow(Zdis, 2)) - 3;

            if (distance <= 0)
            {
                return true;
            }
            return false;
        }

        private bool Bumper2()
        {
            if (_pinballLocation.X >= _bumper2Location.X)
            {
                Xdis = _pinballLocation.X - _bumper2Location.X;
            }
            else
            {
                Xdis = _bumper2Location.X - _pinballLocation.X;
            }

            if (_pinballLocation.Z >= _bumper2Location.Z)
            {
                Zdis = _pinballLocation.Z - _bumper2Location.Z;
            }
            else
            {
                Zdis = _bumper2Location.Z - _pinballLocation.Z;
            }

            distance = Math.Sqrt(Math.Pow(Xdis, 2) + Math.Pow(Zdis, 2)) - 3;

            if (distance <= 0)
            {
                return true;
            }
            return false;
        }

        private bool Bumper3()
        {
            if (_pinballLocation.X >= _bumper3Location.X)
            {
                Xdis = _pinballLocation.X - _bumper3Location.X;
            }
            else
            {
                Xdis = _bumper3Location.X - _pinballLocation.X;
            }

            if (_pinballLocation.Z >= _bumper3Location.Z)
            {
                Zdis = _pinballLocation.Z - _bumper3Location.Z;
            }
            else
            {
                Zdis = _bumper3Location.Z - _pinballLocation.Z;
            }

            distance = Math.Sqrt(Math.Pow(Xdis, 2) + Math.Pow(Zdis, 2)) - 3;

            if (distance <= 0)
            {
                return true;
            }
            return false;
        }

        private bool Bumper4()
        {
            if (_pinballLocation.X >= _bumper4Location.X)
            {
                Xdis = _pinballLocation.X - _bumper4Location.X;
            }
            else
            {
                Xdis = _bumper4Location.X - _pinballLocation.X;
            }

            if (_pinballLocation.Z >= _bumper4Location.Z)
            {
                Zdis = _pinballLocation.Z - _bumper4Location.Z;
            }
            else
            {
                Zdis = _bumper4Location.Z - _pinballLocation.Z;
            }
            
            distance = Math.Sqrt(Math.Pow(Xdis, 2) + Math.Pow(Zdis, 2)) - 3;

            if (distance <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region MovingBumper-Bool-Funktionen
        private bool MovingBumper1()
        {
            if (_pinballLocation.X >= _movingBumper1Location.X)
            {
                Xdis = _pinballLocation.X - _movingBumper1Location.X;
            }
            else
            {
                Xdis = _movingBumper1Location.X - _pinballLocation.X;
            }

            if (_pinballLocation.Z >= _movingBumper1Location.Z)
            {
                Zdis = _pinballLocation.Z - _movingBumper1Location.Z;
            }
            else
            {
                Zdis = _movingBumper1Location.Z - _pinballLocation.Z;
            }

            distance = Math.Sqrt(Math.Pow(Xdis, 2) + Math.Pow(Zdis, 2)) - 3;

            if (distance <= 0)
            {
                return true;
            }
            return false;
        }

        private bool MovingBumper2()
        {
            if (_pinballLocation.X >= _movingBumper2Location.X)
            {
                Xdis = _pinballLocation.X - _movingBumper2Location.X;
            }
            else
            {
                Xdis = _movingBumper2Location.X - _pinballLocation.X;
            }

            if (_pinballLocation.Z >= _movingBumper2Location.Z)
            {
                Zdis = _pinballLocation.Z - _movingBumper2Location.Z;
            }
            else
            {
                Zdis = _movingBumper2Location.Z - _pinballLocation.Z;
            }

            distance = Math.Sqrt(Math.Pow(Xdis, 2) + Math.Pow(Zdis, 2)) - 3;

            if (distance <= 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Grenzen-Bool-Funktionen
        private bool GrenzeRechts()
        {
            if(_pinballLocation.Z <= -39f && _pinballLocation.X <= -15f && _pinballLocation.X >= -21f)
            {
                return true;
            }
            return false;
        }

        private bool GrenzeLinks()
        {
            if (_pinballLocation.Z <= -39f && _pinballLocation.X >= 15f && _pinballLocation.X <= 24f)
            {
                return true;
            }
            return false;
        }

        private bool GrenzeMitte()
        {
            if(_pinballLocation.Z <= -42f && _pinballLocation.X <= 15f && _pinballLocation.X >= -15f)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Startrampe-Bool-Funktionen
        /*
        private bool StartRampeUnten()
        {
            if (_pinballLocation.X >= -23 && _pinballLocation.X <= -22)
            {
                if (_pinballLocation.Z <= 44 && _pinballLocation.Z >= -48)
                {
                    return true;
                }
            }
            return false;
        }

        private bool StartRampeOben()
        {
            if (_pinballLocation.X >= -23 && _pinballLocation.X <= 20)
            {
                if (_pinballLocation.Z <= 48 && _pinballLocation.Z >= 47)
                {
                    return true;
                }
            }
            return false;
        }

        private bool StartRampeLinks()
        {
            if (_pinballLocation.X >= 22 && _pinballLocation.X <= 23)
            {
                if (_pinballLocation.Z <= 48 && _pinballLocation.Z >= 45)
                {
                    return true;
                }
            }
            return false;
        }*/
        #endregion

        #region Startrampenbegrenzung Bool-Funktionen
        private bool rechteWand()
        {
            if(_pinballLocation.X >= -21 && _pinballLocation.X <= -22)
            {
                if(_pinballLocation.Y >= - 23 && _pinballLocation.Y <= 46)
                {
                    return true;
                }
            }
            return false;
        }

        private bool obereWand()
        {
            if(_pinballLocation.X <= 20 && _pinballLocation.X >= -21)
            {
                if(_pinballLocation.Y >= 46 && _pinballLocation.Y <= 47)
                {
                    return true;
                }
            }
            return false;
        }

        private bool linkeWand()
        {
            if(_pinballLocation.X >= 21 && _pinballLocation.X <= 22)
            {
                if(_pinballLocation.Y <= 46 && _pinballLocation.Y >= 36)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region MovingBumper1-Funktion
        bool _movingBumper1Right = true;
        bool _movingBumper1Left = false;

        private void moveMovingBumper1()
        {
            if(_movingBumper1Location.X <= -15f)
            {
                _movingBumper1Right = true;
                _movingBumper1Left = false;
            }

            if(_movingBumper1Location.X >= 15f)
            {
                _movingBumper1Left = true;
                _movingBumper1Right = false;
            }

            if (_movingBumper1Right)
            {
                _movingBumper1Location.X += 0.2f;
            }

            if(_movingBumper1Left)
            {
                _movingBumper1Location.X -= 0.2f;
            }
        }
        #endregion

        #region MovingBumper2-Funktion
        bool _movingBumper2Right = true;
        bool _movingBumper2Left = false;

        private void moveMovingBumper2()
        {
            if (_movingBumper2Location.X <= -15f)
            {
                _movingBumper2Right = true;
                _movingBumper2Left = false;
            }

            if (_movingBumper2Location.X >= 15f)
            {
                _movingBumper2Left = true;
                _movingBumper2Right = false;
            }

            if (_movingBumper2Right)
            {
                _movingBumper2Location.X += 0.2f;
            }

            if (_movingBumper2Left)
            {
                _movingBumper2Location.X -= 0.2f;
            }
        }
        #endregion

        /*public Rectangle Box(int x, int y, int breite, int höhe)
        {
            Rectangle collision = new Rectangle(x, y, breite, höhe);
            return collision;
        }*/

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(0, 0), Color.Black);
            spriteBatch.End();

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
            Matrix _startRampeMatrix = Matrix.CreateTranslation(_startRampeLocation);
            Matrix _grenzeRechtsMatrix = Matrix.CreateTranslation(_grenzeRechtsLocation);
            Matrix _grenzeLinksMatrix = Matrix.CreateTranslation(_grenzeLinksLocation);
            Matrix _movingBumper1Matrix = Matrix.CreateTranslation(_movingBumper1Location);
            Matrix _movingBumper2Matrix = Matrix.CreateTranslation(_movingBumper2Location);
            //Matrix _bumperWand1Matrix = Matrix.CreateTranslation(_bumperWand1Location);

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
            DrawModel(_startRampe, _startRampeMatrix, viewMatrix, projectionMatrix);
            DrawModel(_grenzeRechts, _grenzeRechtsMatrix, viewMatrix, projectionMatrix);
            DrawModel(_grenzeLinks, _grenzeLinksMatrix, viewMatrix, projectionMatrix);
            DrawModel(_movingBumper1, _movingBumper1Matrix, viewMatrix, projectionMatrix);
            DrawModel(_movingBumper2, _movingBumper2Matrix, viewMatrix, projectionMatrix);
            //DrawModel(_bumperWand1, _bumperWand1Matrix, viewMatrix, projectionMatrix);

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
                    //effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    //effect.EnableDefaultLighting();
                    //effect.DiffuseColor = new Vector3(10, 0, 0);
                    effect.View = view;
                    effect.World = world;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}