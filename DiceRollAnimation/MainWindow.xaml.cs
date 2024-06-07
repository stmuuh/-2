using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiceRollAnimation
{
    public partial class MainWindow : Window
    {
        private Model3DGroup diceModelGroup;
        private Random random;

        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            SetupDiceModel();
            StartDiceAnimation();
        }
        private void SetupDiceModel()
        {
            diceModelGroup = new Model3DGroup();

            Material dice1Material = new DiffuseMaterial((Brush)FindResource("Dice1"));
            Material dice2Material = new DiffuseMaterial((Brush)FindResource("Dice2"));
            Material dice3Material = new DiffuseMaterial((Brush)FindResource("Dice3"));
            Material dice4Material = new DiffuseMaterial((Brush)FindResource("Dice4"));
            Material dice5Material = new DiffuseMaterial((Brush)FindResource("Dice5"));
            Material dice6Material = new DiffuseMaterial((Brush)FindResource("Dice6"));

            MeshGeometry3D diceMesh = new MeshGeometry3D();

            Point3DCollection points = new Point3DCollection
            {
                new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(1, 1, -1), new Point3D(-1, 1, -1),
                new Point3D(-1, -1, 1), new Point3D(1, -1, 1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1)
            };

            PointCollection textureCoords = new PointCollection
            {
                new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1),
                new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1)
            };

            Int32Collection triangles = new Int32Collection
            {
                // Front face
                0, 1, 2, 0, 2, 3,
                // Back face
                4, 6, 5, 4, 7, 6,
                // Left face
                4, 5, 1, 4, 1, 0,
                // Right face
                3, 2, 6, 3, 6, 7,
                // Top face
                1, 5, 6, 1, 6, 2,
                // Bottom face
                4, 0, 3, 4, 3, 7
            };

            diceMesh.Positions = points;
            diceMesh.TriangleIndices = triangles;
            diceMesh.TextureCoordinates = textureCoords;

            GeometryModel3D frontFace = new GeometryModel3D(diceMesh, dice1Material);
            GeometryModel3D backFace = new GeometryModel3D(diceMesh, dice6Material);
            GeometryModel3D leftFace = new GeometryModel3D(diceMesh, dice4Material);
            GeometryModel3D rightFace = new GeometryModel3D(diceMesh, dice3Material);
            GeometryModel3D topFace = new GeometryModel3D(diceMesh, dice2Material);
            GeometryModel3D bottomFace = new GeometryModel3D(diceMesh, dice5Material);

            diceModelGroup.Children.Add(frontFace);
            diceModelGroup.Children.Add(backFace);
            diceModelGroup.Children.Add(leftFace);
            diceModelGroup.Children.Add(rightFace);
            diceModelGroup.Children.Add(topFace);
            diceModelGroup.Children.Add(bottomFace);

            ModelVisual3D modelVisual = new ModelVisual3D();
            modelVisual.Content = diceModelGroup;
            viewport3D.Children.Add(modelVisual);
        }
        private void StartDiceAnimation()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) => AnimateDiceRoll();
            timer.Start();
        }

        private void AnimateDiceRoll()
        {
            int angleX = random.Next(0, 360);
            int angleY = random.Next(0, 360);
            int angleZ = random.Next(0, 360);

            AxisAngleRotation3D rotationX = new AxisAngleRotation3D(new Vector3D(1, 0, 0), angleX);
            AxisAngleRotation3D rotationY = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angleY);
            AxisAngleRotation3D rotationZ = new AxisAngleRotation3D(new Vector3D(0, 0, 1), angleZ);

            Transform3DGroup transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(new RotateTransform3D(rotationX));
            transformGroup.Children.Add(new RotateTransform3D(rotationY));
            transformGroup.Children.Add(new RotateTransform3D(rotationZ));

            diceModelGroup.Transform = transformGroup;
        }
    }
}