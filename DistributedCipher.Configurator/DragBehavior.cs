using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DistributedCipher.Configurator
{
    public class DragBehavior : Behavior<FrameworkElement>
    {
        protected bool isMouseClicked;

        public DragBehavior()
        {
            this.isMouseClicked = false;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.MouseLeave += new MouseEventHandler(AssociatedObject_MouseLeave);
            this.AssociatedObject.MouseLeftButtonDown += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonDown);
            this.AssociatedObject.MouseLeftButtonUp += new MouseButtonEventHandler(AssociatedObject_MouseLeftButtonUp);
        }

        public void AssociatedObject_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.isMouseClicked)
            {
                IDragable draggableObject = this.AssociatedObject as IDragable;

                if (draggableObject != null)
                {
                    DataObject data = new DataObject();
                    
                    data.SetData(draggableObject.DataType, this.AssociatedObject.DataContext);

                    DragDrop.DoDragDrop(this.AssociatedObject, data, DragDropEffects.Move);
                }
            }

            this.isMouseClicked = false;
        }

        public void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.isMouseClicked = true;
        }

        public void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isMouseClicked = false;
        }
    }
}
