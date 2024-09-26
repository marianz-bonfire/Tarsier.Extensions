using System.Reflection;
using System.Windows.Forms;

namespace Tarsier.Extensions
{
    public static class Controls
    {

        public static void AddUserControl(this Panel container, UserControl control) {
            if (container.InvokeRequired) {
                container.Invoke((MethodInvoker)delegate () {
                    container.SafeClearControls();
                    container.Controls.Add(control);
                });
            } else {
                container.SafeClearControls();
                container.Controls.Add(control);
            }
        }
        public static string GetSelectedSubSelections(this Panel panel) {
            string selectedTag = string.Empty;
            foreach (Control control in panel.Controls) {
                if (control is RadioButton) {
                    RadioButton rb = control as RadioButton;
                    if (rb.Checked) {
                        string tag = rb.Tag.ToSafeString();
                        if (!string.IsNullOrWhiteSpace(tag)) {
                            selectedTag = tag;
                            break;
                        }
                    }
                }
            }
            return selectedTag;
        }

        public static void SafeClearControls(this Panel panel) {
            //This will help to avoid error "System.ComponentModel.Win32Exception: Error creating window handle"
            // in adding new control in the panel
            for (int i = panel.Controls.Count - 1; i >= 0; --i) {
                var control = panel.Controls[i];
                if (control != null) {
                    control.Dispose();
                }
            }
            if (panel.IsHandleCreated) {
                panel?.Invoke((MethodInvoker)(() => panel.Controls.Clear()));
            }
        }


        public static void SetDoubleBuffered(this Control control) {
            PropertyInfo pi = control.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi == null)
                return;
            pi.SetValue(control, true, null);
        }

        public static void DoubleBufferControls(this Control control) {
            if (control.Controls.Count > 0) {
                foreach (Control child in control.Controls) {
                    SetDoubleBuffered(child);
                    if (child.Controls.Count > 0)
                        DoubleBufferControls(child);
                }
            }
        }

        public static T GetParent<T>(this Control ctl) where T : Control {
            var retVal = default(T);
            var parent = ctl;
            while (parent.Parent != null) {
                parent = parent.Parent;
                if (parent is T) {
                    retVal = (T)parent;
                    break;
                }
            }
            return retVal;
        }
    }
}
