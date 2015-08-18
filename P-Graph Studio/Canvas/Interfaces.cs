/* Copyright 2015 Department of Computer Science and Systems Technology, University of Pannonia

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

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