using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSGBD;

namespace KineGestionApp
{
    public static partial class GestionPrescriptions
    {
        /// <summary>
        /// Définit tout gestionnaire de prescription
        /// </summary>
        public interface IPrescription
        {
            /// <summary>
            /// Enumère tous les prescriptions existantes
            /// </summary>
            /// <returns>Énumération des prescriptions</returns>
            IEnumerable<ModelesPrescriptions.IPrescription> EnumererPrescriptions();

            /// <summary>
            /// Permet de charger une prescription selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de la prescription</param>
            /// <returns>Médecin chargé si possible, sinon null</returns>
            ModelesPrescriptions.IPrescription ChargerPrescriptions(int id);

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IPrescription
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IPrescription</returns>
            ModelesPrescriptions.IPrescription CreerPrescriptions();

            /// <summary>
            /// Permet de mettre à jour (au sein du support d'informations) la prescription spécifiée
            /// </summary>
            /// <param name="prescription">Prescription à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            bool MettreAJour(ModelesPrescriptions.IPrescription prescription);

            /// <summary>
            /// Permet de supprimer définitivement (au sein du support d'informations) la prescription spécifiée
            /// </summary>
            /// <param name="prescription">Medecin à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            bool Supprimer(ModelesPrescriptions.IPrescription prescription);
        }

        public static IPrescription CreerPrescriptiontEnDB() => new PrescriptionEnDB();

        /// <summary>
        /// Implémente un gestionnaire des patients utilisant uniquement la mémoire comme support d'informations
        /// </summary>
        private class PrescriptionEnDB : IPrescription, IDisposable
        {

            /// <summary>
            /// Créer un nouvelle prescription en DB
            /// </summary>
            private static ModelesPrescriptions.IPrescription[] ParDefaut { get; } = new ModelesPrescriptions.IPrescription[]
            {
                ModelesPrescriptions.CreerNouvellePrescription()
            };

            private Dictionary<int, ModelesPrescriptions.IPrescription> enDB { get; } = new Dictionary<int, ModelesPrescriptions.IPrescription>();

            /// <summary>
            /// Enumère tous les prescriptions
            /// </summary>
            /// <returns>Énumération des prescriptions</returns>
            public IEnumerable<ModelesPrescriptions.IPrescription> EnumererPrescriptions()
            {
                return enDB.Values.OrderBy(prescription => prescription.NumeroPrescription[0]);
            }

            /// <summary>
            /// Permet de charger une prescription selon l'identifiant spécifié
            /// </summary>
            /// <param name="id">Identifiant de cette séance</param>
            /// <returns>Prescription chargée si possible, sinon null</returns>
            public ModelesPrescriptions.IPrescription ChargerPrescriptions(int id)
            {
                return enDB.TryGetValue(id, out var prescription) ? prescription : null;
            }

            /// <summary>
            /// Permet de retourner une nouvelle entité de type IPrescription
            /// <para>Ces données ne sont pas encore définies et valides à ce stade !</para>
            /// </summary>
            /// <returns>Nouvelle entité de type IPrescription</returns>
            public ModelesPrescriptions.IPrescription CreerPrescriptions()
            {
                return ModelesPrescriptions.CreerNouvellePrescription();
            }

            /// <summary>
            /// Permet de mettre à jour (en DB) la prescription spécifiée
            /// </summary>
            /// <param name="prescription">Prescriptionz à mettre jour</param>
            /// <returns>Vrai si la mise à jour a pu être réalisée (par création ou modification), sinon faux</returns>
            public bool MettreAJour(ModelesPrescriptions.IPrescription prescription)
            {
                if (prescription == null) return false;
                if (prescription.Id < 1)
                {
                    // Ajout si il est valide
                    prescription.DefinirIdPrescription((enDB.Count == 0) ? 1 : enDB.Keys.Max() + 1);
                    if (prescription.EstValide())
                    {
                        enDB.Add(prescription.Id, prescription);
                    }
                }
                else
                {
                    // Modification
                    // ici, rien à faire car le chargement d'un patient existant ne fait que retourner la référence d'un objet déjà présent dans le dictionnaire (la BD)
                }
                return true;
            }

            /// <summary>
            /// Permet de supprimer définitivement (en DB) la prescription spécifiée
            /// </summary>
            /// <param name="prescription">Prescription à supprimer</param>
            /// <returns>Vrai si la suppression a pu être réalisée, sinon faux</returns>
            public bool Supprimer(ModelesPrescriptions.IPrescription prescription)
            {
                if (prescription == null) return false;
                return enDB.Remove(prescription.Id);
            }


            public PrescriptionEnDB()
            {
                ModelesPrescriptions.SurChangementNumeroPrescription += SurChangementNumeroPrescription;
                ModelesPrescriptions.SurChangementNombrePrescription += SurChangementNombrePrescription;
                ModelesPrescriptions.SurChangementAcomptePrescription += SurChangementAcomptePrescription;
                ModelesPrescriptions.SurChangementDatePrescription += SurChangementDatePrescription;
                ModelesPrescriptions.SurChangementStatutPrescription += SurChangementStatutPrescription;
                ModelesPrescriptions.SurChangementDatePrescription += SurChangementDatePrescription;

                enDB.Clear();
                foreach (var enregistrementPrescription in ParDefaut)
                {
                    var enregistrementNouvellePrescription = ModelesPrescriptions.CreerPrescription(enregistrementPrescription.Id, enregistrementPrescription.NombreSeances, enregistrementPrescription.NumeroPrescription, enregistrementPrescription.Acompte, enregistrementPrescription.DatePrescription, enregistrementPrescription.Cloturee);
                }
            }

            ~PrescriptionEnDB()
            {
                Dispose();
            }

            public void Dispose()
            {
                ModelesPrescriptions.SurChangementNumeroPrescription -= SurChangementNumeroPrescription;
                ModelesPrescriptions.SurChangementNombrePrescription -= SurChangementNombrePrescription;
                ModelesPrescriptions.SurChangementAcomptePrescription -= SurChangementAcomptePrescription;
                ModelesPrescriptions.SurChangementDatePrescription -= SurChangementDatePrescription;
                ModelesPrescriptions.SurChangementStatutPrescription -= SurChangementStatutPrescription;
                ModelesPrescriptions.SurChangementDatePrescription += SurChangementDatePrescription;
            }


            private void SurChangementNumeroPrescription(ModelesPrescriptions.IPrescription prescription, string valeurActuelle, string nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !prescription.Id.Equals(prescription.Id)
                && prescription.NumeroPrescription.Equals(nouvelleValeur, StringComparison.CurrentCultureIgnoreCase)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementNombrePrescription(ModelesPrescriptions.IPrescription prescription, int valeurActuelle, int nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !prescription.Id.Equals(prescription.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementAcomptePrescription(ModelesPrescriptions.IPrescription prescription, SqlMoney valeurActuelle, SqlMoney nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !prescription.Id.Equals(prescription.Id)))
                {
                    annulation.Cancel();
                }
            }
            private void SurChangementStatutPrescription(ModelesPrescriptions.IPrescription prescription, bool valeurActuelle, bool nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !prescription.Id.Equals(prescription.Id)))
                {
                    annulation.Cancel();
                }
            }

            private void SurChangementDatePrescription(ModelesPrescriptions.IPrescription prescription, DateTime valeurActuelle, DateTime nouvelleValeur, CancellationToken annulation)
            {
                if (enDB.Values.Any(prescriptionEnDB => !prescription.Id.Equals(prescription.Id)))
                {
                    annulation.Cancel();
                }
            }
        }
    }
}
