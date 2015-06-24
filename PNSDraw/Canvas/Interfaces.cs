using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PNSDraw.Canvas
{
    public interface IGraphicsObject
    {
        void SetID(int newid);
        int GetID();
        void Draw(Graphics g, bool plain);
        void DrawGhost(Graphics g);
        bool HitTest(Point mousecoords);
        void SetSelected(bool selected);
        bool IsSelected();
        bool IntersectsWith(Rectangle rect);
        void SetEditSelected(bool selected);
        int GetLayer();
        bool IsPartialObject();
        IGraphicsObject GetParentObject();
        void SetSelectedChild(IGraphicsObject child);
        void SetHighlighted(bool highlighted);
        bool IsHighlighted();

        Point GetCoords();
        Point GetCurrentCoords();
        Point GetOffset();
        void SetCoords(Point newcoords);
        void SetOffset(Point newoffset);
        void IntegrateOffset();
        Rectangle GetBoundary();

        void Pin(int fp);
        int getPin();

        bool IsMoveable();
        bool IsDeletable();

        bool IsLocked();
    }

    public interface IConnectableObject
    {
        bool IsValidConnectorBegin(IConnectableObject end);
        bool IsValidConnectorEnd(IConnectableObject begin);
        Point GetCurrentConnectorBeginCoords();
        Point GetCurrentConnectorEndCoords();
        Point GetConnectorBeginCoords();
        Point GetConnectorEndCoords();
        void AddConnection(IConnectableObject begin);
        void RemoveConnection(IConnectableObject begin);
    }


    public interface IGraphicsStructure
    {
        List<IGraphicsObject> GetObjectList();
        void AddSingleObject(IGraphicsObject obj);
        void RemoveSingleObject(IGraphicsObject obj);
        void AddSingleGraphicsObject(IGraphicsObject obj);
        void RemoveSingleGraphicsObject(IGraphicsObject obj);
        IGraphicsObject GetObjectByID(int objectid);
        int GetFreeUniqueID();
        string ValidateName(IGraphicsObject obj, string name);
        string GenerateName(IGraphicsObject obj);
    }

}