//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();
        public bool Cooked { get; private set; } = false;

        public Product FinalProduct { get; set; }

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }
        
        public int GetCookTime()
        {
            int tiempo_de_cocina = 0;
            foreach (BaseStep pasito in this.steps)
                {
                    tiempo_de_cocina += pasito.Time;
                }
            return tiempo_de_cocina*1000;
       }

       public void Cook()
       {
        CountdownTimer cronometro = new CountdownTimer();
        cronometro.Register (GetCookTime(), new RecipeAux (this));
       }
       public class RecipeAux : TimerClient
    {
        public Recipe recipee;
        public RecipeAux (Recipe recipe)
        {
            this.recipee = recipe;
        }
        public void TimeOut()
        {
            this.recipee.Cooked = true;
        }
    }
    }
}

