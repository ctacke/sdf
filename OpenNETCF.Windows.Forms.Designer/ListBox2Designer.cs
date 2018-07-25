using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using Microsoft.CompactFramework.Design;
//using OpenNETCF.Windows.Forms;
using System.Reflection;

namespace OpenNETCF.Windows.Forms
{
    public class ListBox2Designer : DeviceUserControlDesigner
    {
        private ListBox2 listBox;

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            listBox = component as ListBox2;
            IComponentChangeService componentChangeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            RegisterChangeNotifications(componentChangeService);
        }

        //// This override allows the control to register event handlers for IComponentChangeService events
        //// at the time the control is sited, which happens only in design mode.
        //public override ISite Site
        //{
        //    get
        //    {
        //        return base.Site;
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {

        //            ClearChangeNotifications();
        //            return;
        //        }

        //        base.Site = value;
        //        // Clear any component change event handlers.
        //        ClearChangeNotifications();

        //        RegisterChangeNotifications();

        //        //designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
        //    }
        //}

        //private void ClearChangeNotifications()
        //{
        //    // The m_changeService value is null when not in design mode, 
        //    // as the IComponentChangeService is only available at design time.    
        //    m_changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

        //    // Clear our the component change events to prepare for re-siting.                
        //    if (m_changeService != null)
        //    {
        //        m_changeService.ComponentChanged -= new ComponentChangedEventHandler(OnComponentChanged);
        //        //				//m_changeService.ComponentChanging -= new ComponentChangingEventHandler(OnComponentChanging);
        //        m_changeService.ComponentAdded -= new ComponentEventHandler(OnComponentAdded);
        //        //m_changeService.ComponentAdding -= new ComponentEventHandler(OnComponentAdding);
        //        m_changeService.ComponentRemoved -= new ComponentEventHandler(OnComponentRemoved);
        //        m_changeService.ComponentRemoving -= new ComponentEventHandler(OnComponentRemoving);
        //        //m_changeService.ComponentRename -= new ComponentRenameEventHandler(OnComponentRename);
        //    }
        //}

        private void RegisterChangeNotifications(IComponentChangeService m_changeService)
        {
            //m_changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            // Register the event handlers for the IComponentChangeService events
            if (m_changeService != null)
            {

                m_changeService.ComponentChanged += new ComponentChangedEventHandler(OnComponentChanged);
                //				//m_changeService.ComponentChanging += new ComponentChangingEventHandler(OnComponentChanging);
                m_changeService.ComponentAdded += new ComponentEventHandler(OnComponentAdded);
                //m_changeService.ComponentAdding += new ComponentEventHandler(OnComponentAdding);
                m_changeService.ComponentRemoved += new ComponentEventHandler(OnComponentRemoved);
                m_changeService.ComponentRemoving += new ComponentEventHandler(OnComponentRemoving);
                //m_changeService.ComponentRename += new ComponentRenameEventHandler(OnComponentRename);
            }
        }

        //private void OnComponentAdding(object sender, ComponentEventArgs ce)
        //{


        //}

        private void OnComponentAdded(object sender, ComponentEventArgs ce)
        {

            if (ce.Component.Site.Component.GetType() == typeof(ListItem))
            {
                //ListBox2 listBox = this.Control as ListBox2;
                if (listBox != null)
                {
                    listBox.Refresh();
                }
            }
        }

        private void OnComponentRemoved(object sender, ComponentEventArgs ce)
        {
            if (ce.Component.Site.Component.GetType() == typeof(ListItem))
            {
                //ListBox2 listBox = this.Control as ListBox2;
                if (listBox != null)
                {
                    listBox.Refresh();
                }
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
        {
            if (ce.Component.GetType() == typeof(ListBox2) || ce.Component.GetType() == typeof(ListItem))
            {
                //MessageBox.Show("OnComponentChanged");
                //ListBox2 listBox = this.Control as ListBox2;

                if (listBox != null)
                {
                    if (listBox.Items.Count > 0)
                    {
                        listBox.Refresh();
                    }
                }
            }
        }

        private void OnComponentRemoving(object sender, ComponentEventArgs ce)
        {
            IComponentChangeService c = (IComponentChangeService)GetService(typeof(IComponentChangeService));
            ListItem item;
            IDesignerHost h = (IDesignerHost)GetService(typeof(IDesignerHost));
            IContainer cont = ce.Component.Site.Container;
            //ListBox2 listBox = this.Control as ListBox2;

            if (listBox == null)
                return;

            listBox.Refresh();
            int i;

            // If the user is removing the control itself
            if (ce.Component == this)
            {
                for (i = listBox.Items.Count - 1; i >= 0; i--)
                {
                    item = listBox.Items[i];
                    //c.OnComponentChanging(MyControl, null);
                    //cont.Remove(item);
                    listBox.Items.Remove(item);
                    h.DestroyComponent(item);
                    //c.OnComponentChanged(MyControl, null, null, null);
                }
            }

            if (ce.Component.Site.Component.GetType() == typeof(ListItem))
            {
                //this.Items.Remove((ListItem)ce.Component.Site.Component);
                listBox.Invalidate();
            }
        }


    }
}
