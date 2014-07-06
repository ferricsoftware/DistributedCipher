using System;
using System.Windows;
using System.Windows.Interactivity;

namespace DistributedCipher.Configurator
{
    public class DropBehavior : Behavior<FrameworkElement>
    {
        protected Type dataType;
        protected FrameworkElementAdorner adorner;

        public DropBehavior()
        {
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.AllowDrop = true;
            this.AssociatedObject.DragEnter += new DragEventHandler(AssociatedObject_DragEnter);
            this.AssociatedObject.DragLeave += new DragEventHandler(AssociatedObject_DragLeave);
            this.AssociatedObject.DragOver += new DragEventHandler(AssociatedObject_DragOver);
            this.AssociatedObject.Drop += new DragEventHandler(AssociatedObject_Drop);
        }

        public void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            if (this.dataType == null)
            {
                if (this.AssociatedObject.DataContext != null)
                {
                    IDropable dropObject = this.AssociatedObject.DataContext as IDropable;

                    if (dropObject != null)
                        this.dataType = dropObject.DataType;
                }
            }

            if (this.adorner != null)
                this.adorner = new FrameworkElementAdorner(sender as UIElement);

            e.Handled = true;
        }

        public void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            if (this.adorner != null)
                this.adorner.Remove();

            e.Handled = true;
        }

        public void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            if (this.dataType != null)
            {
                //if item can be dropped
                if (e.Data.GetDataPresent(dataType))
                {
                    this.SetDragDropEffects(e);

                    if (this.adorner != null)
                        this.adorner.Update();
                }
            }
        }

        public void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if(this.dataType != null)
            {
                if (e.Data.GetDataPresent(this.dataType))
                {
                    IDropable target = this.AssociatedObject.DataContext as IDropable;
                    target.Drop(e.Data.GetData(this.dataType));

                    IDragable source = e.Data.GetData(this.dataType) as IDragable;
                    source.Remove(e.Data.GetData(this.dataType));
                }
            }

            if (this.adorner != null)
                this.adorner.Remove();

            e.Handled = true;

            return;
        }

        protected void SetDragDropEffects(DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            if (e.Data.GetDataPresent(this.dataType))
                e.Effects = DragDropEffects.Move;
        }
    }
}
