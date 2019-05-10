using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class AIEasyPlayer : AIPlayer
    {
        private enum AIStates
        {
            Searching,
            RandomTarget,
        }
        private AIStates _CurrentState = AIStates.Searching;
        private Stack<Location> _RTargets = new Stack<Location>();

        public AIEasyPlayer(BattleShipsGame controller) : base(controller)
        { }

        protected override void GenerateCoords(ref int row, ref int column)
        {
            do
            {
                switch (_CurrentState)
                {
                    case AIStates.Searching:
                        SearchCoords(ref row, ref column);
                        break;
                    case AIStates.RandomTarget:
                        TargetCoords(ref row, ref column);
                        break;
                    default:
                        throw new ApplicationException("AI has gone in an invalid state");
                }
            } while ((row < 0 || column < 0 || row >= EnemyGrid.Height || column >= EnemyGrid.Width || EnemyGrid[row, column] != TileView.Sea));
        }
        private void TargetCoords(ref int row, ref int column)
        {
            Location l = _RTargets.Pop();

            if ((_RTargets.Count == 0))
                _CurrentState = AIStates.Searching;
            row = l.Row;
            column = l.Column;
        }
        private void SearchCoords(ref int row, ref int column)
        {
            row = _Random.Next(0, EnemyGrid.Height);
            column = _Random.Next(0, EnemyGrid.Width);
        }

        protected override void ProcessShot(int row, int col, AttackResult result)
        {
            if (result.Value == ResultOfAttack.Hit)
            {
                _CurrentState = AIStates.RandomTarget;
                AddTarget(row - _Random.Next(0, EnemyGrid.Height), col);
                AddTarget(row, col - _Random.Next(0, EnemyGrid.Width));
                AddTarget(row + _Random.Next(0, EnemyGrid.Height), col);
                AddTarget(row, col + _Random.Next(0, EnemyGrid.Width));
            }
            else if (result.Value == ResultOfAttack.ShotAlready)
            {
                throw new ApplicationException("Error in AI");
            }
        }

        private void AddTarget(int row, int column)
        {

            if (row >= 0 && column >= 0 && row < EnemyGrid.Height && column < EnemyGrid.Width && EnemyGrid[row, column] == TileView.Sea)
            {
                _RTargets.Push(new Location(row, column));
            }
        }
    }

