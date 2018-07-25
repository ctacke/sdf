using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using Microsoft.CompactFramework.Design;
using System.Reflection;

namespace OpenNETCF.Windows.Forms
{
    public class SmartListDesigner : DeviceUserControlDesigner
    {

        private SmartList listBox;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            listBox = component as SmartList;
            IComponentChangeService componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            RegisterChangeNotifications(componentChangeService);
        }
        private void RegisterChangeNotifications(IComponentChangeService m_changeService)
        {
            // Register the event handlers for the IComponentChangeService events
            if (m_changeService != null)
            {

                m_changeService.ComponentChanged += new ComponentChangedEventHandler(OnComponentChanged);
                m_changeService.ComponentAdded += new ComponentEventHandler(OnComponentAdded);               
                m_changeService.ComponentRemoved += new ComponentEventHandler(OnComponentRemoved);
                m_changeService.ComponentRemoving += new ComponentEventHandler(OnComponentRemoving);
            }
        }


        private void OnComponentAdded(object sender, ComponentEventArgs ce)
        {

            if (ce.Component.Site.Component.GetType() == typeof(SmartListItem))
            {
                //MessageBox.Show("A component, " + ce.Component.Site.Name + ", has been added.");
                SmartListItem item = (SmartListItem)ce.Component.Site.Component;
                //item.imageList = imageList;
                //item.Parent = this;
                //MessageBox.Show(this.Items.Count.ToString());
                //if (!this.Items.Contains(item))
                //    this.Items.Add(item);
                //SmartList listBox = this.Control as SmartList;
                if (listBox != null)
                {
                    listBox.Refresh();
                }

            }
        }

        private void OnComponentRemoved(object sender, ComponentEventArgs ce)
        {
            if (ce.Component.Site.Component.GetType() == typeof(SmartListItem))
            {
                //SmartList listBox = this.Control as SmartList;

                if (listBox != null)
                {
                    listBox.Refresh();
                }
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
        {
            if (ce.Component.GetType() == typeof(SmartList) || ce.Component.GetType() == typeof(SmartListItem))
            {
                //MessageBox.Show("OnComponentChanged");
               // SmartList listBox = this.Control as SmartList;

                if (listBox != null)
                {
                    if (listBox.Items.Count > 0)
                        listBox.Refresh();
                }
            }
        }

        private void OnComponentRemoving(object sender, ComponentEventArgs ce)
        {
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            SmartListItem item;
            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
            IContainer cont = ce.Component.Site.Container;
            int i;

            //SmartList listBox = this.Control as SmartList;
            if (listBox != null)
                return;
            // If the user is removing the control itself
            if (ce.Component == this)
            {
                for (i = listBox.Items.Count - 1; i >= 0; i--)
                {
                    item = listBox.Items[i];
                    cont.Remove(item);
                    listBox.Items.Remove(item);
                }
            }

            if (ce.Component.Site.Component.GetType() == typeof(SmartListItem))
            {
                //this.Items.Remove((SmartListItem)ce.Component.Site.Component);
                listBox.Invalidate();
            }
        }



    }
}
