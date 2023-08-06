using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLSite.Models.Library
{
    public class HierarckyNode
    {
        public int SomeNodeData { get; set; }

        public HierarckyNode Parent { get { return m_Parent; } }
        public HierarckyNode PrevSibling { get { return m_PrevSibling; } }
        public HierarckyNode NextSibling { get { return m_NextSibling; } }
        public HierarckyNode FirstChild { get { return m_FirstChild; } }
        public HierarckyNode LastChild { get { return m_LastChild; } }
        public bool Root { get { return Parent == null; } }

        /// <summary>
        /// Remove this and all its children from the tree.
        /// It will become root of a separate tree.
        /// </summary>
        public void Remove()
        {
            // I already am root of separate tree.
            if (Parent == null)
                return;

            if (m_PrevSibling != null)
                m_PrevSibling.m_NextSibling = m_NextSibling;
            else
                m_Parent.m_FirstChild = m_NextSibling;

            if (m_NextSibling != null)
                m_NextSibling.m_PrevSibling = m_PrevSibling;
            else
                m_Parent.m_LastChild = m_PrevSibling;

            m_PrevSibling = null;
            m_NextSibling = null;
            m_Parent = null;
        }

        /// <summary>
        /// Clear children list.
        /// Child HierarckyNodes will be in invalid state!
        /// </summary>
        public void RemoveAllChildren()
        {
            m_FirstChild = m_LastChild = null;
        }

        /// <summary>
        /// Insert given HierarckyNode as first child of this HierarckyNode.
        /// </summary>
        /// <param name="newHierarckyNode">Must be valid root of separate tree. Can have children.</param>
        public void InsertFirstChild(HierarckyNode newNode)
        {
            newNode.m_Parent = this;
            newNode.m_NextSibling = m_FirstChild;

            if (m_FirstChild != null)
                m_FirstChild.m_PrevSibling = newNode;
            else
                m_LastChild = newNode;

            m_FirstChild = newNode;
        }

        /// <summary>
        /// Insert given node as last child of this node.
        /// </summary>
        /// <param name="newNode">Must be valid root of separate tree. Can have children.</param>
        public void InsertLastChild(HierarckyNode newNode)
        {
            newNode.m_Parent = this;
            newNode.m_PrevSibling = m_LastChild;

            if (m_LastChild != null)
                m_LastChild.m_NextSibling = newNode;
            else
                m_FirstChild = newNode;

            m_LastChild = newNode;
        }

        /// <summary>
        /// Insert given node as previous sibling of this node.
        /// </summary>
        /// <param name="newNode">Must be valid root of separate tree. Can have children.</param>
        public void InsertPrevSibling(HierarckyNode newNode)
        {
            newNode.m_Parent = m_Parent;
            newNode.m_PrevSibling = m_PrevSibling;
            newNode.m_NextSibling = this;

            if (m_PrevSibling != null)
                m_PrevSibling.m_NextSibling = newNode;
            else
                m_Parent.m_FirstChild = newNode;

            m_PrevSibling = newNode;
        }

        /// <summary>
        /// Insert given node as next sibling of this node.
        /// </summary>
        /// <param name="newNode">Must be valid root of separate tree. Can have children.</param>
        public void InsertNextSibling(HierarckyNode newNode)
        {
            newNode.m_Parent = m_Parent;
            newNode.m_NextSibling = m_NextSibling;
            newNode.m_PrevSibling = this;

            if (m_NextSibling != null)
                m_NextSibling.m_PrevSibling = newNode;
            else
                m_Parent.m_LastChild = newNode;

            m_NextSibling = newNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if this node is valid comparing to its surrounding.</returns>
        public bool NodeValid()
        {
            // This is root node.
            if (Parent == null)
            {
                // Must not have siblings.
                if (PrevSibling != null || NextSibling != null)
                    return false;
            }
            // This is not root node.
            else
            {
                // Parent must have some children.
                if (Parent.FirstChild == null || Parent.LastChild == null)
                    return false;
            }

            if (PrevSibling != null)
            {
                // My prev sibling must point to me.
                if (PrevSibling.NextSibling != this)
                    return false;
                // My prev sibling must point to same parent.
                if (PrevSibling.Parent != Parent)
                    return false;
                // I can't be first child of my parent.
                if (Parent != null && Parent.FirstChild == this)
                    return false;
            }
            else
            {
                // I must be first child of my parent.
                if (Parent != null && Parent.FirstChild != this)
                    return false;
            }

            if (NextSibling != null)
            {
                // My next sibling must point to me.
                if (NextSibling.PrevSibling != this)
                    return false;
                // My next sibling must point to same parent.
                if (NextSibling.Parent != Parent)
                    return false;
                // I can't be last child of my parent.
                if (Parent != null && Parent.LastChild == this)
                    return false;
            }
            else
            {
                // I must be last child of my parent.
                if (Parent != null && Parent.LastChild != this)
                    return false;
            }

            // I have some children.
            if (FirstChild != null)
            {
                // I must have both FirstChild and LastChild set.
                if (LastChild == null)
                    return false;

                // My first child must point to me.
                if (FirstChild.Parent != this)
                    return false;
                // My first child must not have prev sibling.
                if (FirstChild.PrevSibling != null)
                    return false;

                // My last child must point to me.
                if (LastChild.Parent != this)
                    return false;
                // My last child must not have next sibling.
                if (LastChild.NextSibling != null)
                    return false;
            }
            // I have no children.
            else
            {
                // I must have neither FirstChild nor LastChild set.
                if (LastChild != null)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if this node and all child nodes recursively are valid.</returns>
        public bool TreeValid()
        {
            if (!NodeValid())
                return false;

            for (HierarckyNode childNode = FirstChild; childNode != null; childNode = childNode.NextSibling)
                if (!childNode.TreeValid())
                    return false;

            return true;
        }

        private HierarckyNode m_Parent;
        private HierarckyNode m_PrevSibling, m_NextSibling;
        private HierarckyNode m_FirstChild, m_LastChild;

        public long uid { get; set; }
        public string name { get; set; }
        public int deeps { get; set; }
        public string icnname { get; set; }
        public int showmode { get; set; }
        public long showorder { get; set; }
        public int replytype { get; set; }
        public int acttype { get; set; }
        public int busitype { get; set; }
        public string linkaddr { get; set; }
        public string phone { get; set; }
    }
}