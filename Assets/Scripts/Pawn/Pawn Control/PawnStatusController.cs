using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawnControl
{
    [System.Serializable]
    public class PawnStatusController
    {
        [SerializeField] private string firstName;
        [SerializeField] private string nickName;
        [SerializeField] private string lastName;
        [SerializeField] private PawnMentalStatus mentalStatus;
        [SerializeField] private PawnPhysicalStatus physicalStatus;
        [SerializeField] private PawnNeedsStatus needsStatus;

        public string FirstName { get => firstName; set => firstName = value; }
        public string NickName { get => nickName; set => nickName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public PawnMentalStatus MentalStatus { get => mentalStatus; set => mentalStatus = value; }
        public PawnPhysicalStatus PhysicalStatus { get => physicalStatus; set => physicalStatus = value; }
        public PawnNeedsStatus NeedsStatus { get => needsStatus; set => needsStatus = value; }

        #region Constructor
        public PawnStatusController(PawnStatusContainer statusContainer = null)
        {
            if (statusContainer == null)
            {
                SetFirstName("NoNameGiven");
                SetNickname("NoNameGiven");
                SetLastName("NoNameGiven");

                MentalStatus = new PawnMentalStatus();
                PhysicalStatus = new PawnPhysicalStatus();
                NeedsStatus = new PawnNeedsStatus();
            }
            else
            {
                SetFirstName(statusContainer.FirstName);
                SetNickname(statusContainer.NickName); 
                SetLastName(statusContainer.LastName);

                if (statusContainer.MentalStatusContainer != null)
                    MentalStatus = new PawnMentalStatus(statusContainer.MentalStatusContainer);
                else
                    MentalStatus = new PawnMentalStatus();
                if(statusContainer.PhysicalStatusContainer != null)
                    PhysicalStatus = new PawnPhysicalStatus(statusContainer.PhysicalStatusContainer);
                else
                    PhysicalStatus = new PawnPhysicalStatus();
                if( statusContainer.NeedsStatusContainer != null)
                    NeedsStatus = new PawnNeedsStatus(statusContainer.NeedsStatusContainer);
                else
                    NeedsStatus = new PawnNeedsStatus();
            }
        }
        #endregion

        #region Set Names
        public void SetFirstName(string newFirstName)
        {
            FirstName = newFirstName;
        }

        public void SetLastName(string newLastName)
        {
            LastName = newLastName;
        }

        public void SetNickname(string newNickName)
        {
            NickName = newNickName;
        }
        #endregion

        #region Status Changes
        public void UpdateMentalStatus(MentalStatus newStatus, int magnitude)
        {
            MentalStatus.UpdateMentalStatus(newStatus, magnitude);
        }

        public void UpdatePhysicalStatus(PhysicalStatus newStatus, int magnitude)
        {
            PhysicalStatus.UpdatePhysicalStatus(newStatus, magnitude);
        }

        public void UpdateNeedStatus(NeedStatus needStatus, int magnitude)
        {
            NeedsStatus.UpdateNeedsStatus(needStatus, magnitude);   
        }

        public void IncrementAllStatuses()
        {
            MentalStatus.IncrementAllStatuses();
            PhysicalStatus.IncrementAllStatuses();
            NeedsStatus.IncrementAllStatuses();
        }
        #endregion

        public float GetHappinessPercent()
        {
            return (float)needsStatus.happiness / (float)needsStatus.statMax;
        }

        public float GetHungerPercent()
        {
            return (float)needsStatus.hunger / (float)needsStatus.statMax;
        }

        public float GetSleepPercent()
        {
            return (float)needsStatus.sleep / (float)needsStatus.statMax;
        }

        public float GetEntertainmentPercent()
        {
            return (float)needsStatus.entertainment / (float)needsStatus.statMax;
        }
    }
}