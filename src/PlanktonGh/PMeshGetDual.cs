﻿using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Plankton;
using Rhino.Geometry;

namespace PlanktonGh
{
    public class PMeshGetDual : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PMeshGetDual()
          : base("Plankton Get Dual", "PlanktonGetDual",
            "Get the dual mesh of a Plankton mesh",
            "Mesh", "Plankton")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_PlanktonMeshParam(), "PMesh", "PMesh", "Plankton mesh to get dual of", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GH_PlanktonMeshParam(), "PMesh", "PMesh", "Dual Plankton mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            PlanktonMesh mesh = null;
            DA.GetData(0, ref mesh);
            DA.SetData(0, mesh.Dual());
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("bef9a6b2-3271-425e-a9c8-c97d345f3508"); }
        }
    }
}
