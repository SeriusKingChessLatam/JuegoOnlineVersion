using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
   public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY){
        List<Vector2Int> r = new List<Vector2Int>();

        int direction = (team == 0) ? 1: -1;

        //uno adelante
        if(board[currentX, currentY + direction] == null){
            r.Add(new Vector2Int(currentX, currentY + direction));
        }

        // dos Adelante
        if(board[currentX, currentY + direction] == null){
            //White Team Equipo Blanco
            if(team == 0 && currentY == 1 && board[currentX, currentY + (direction * 2)]== null){
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            }
            //Black Team Equipo Negro
            if(team == 1 && currentY == 6 && board[currentX, currentY + (direction * 2)]== null){
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            }
        }

        //Comer o Matar enemigo
        //Blancas
        if(currentX != tileCountX-1){
            if(board[currentX + 1, currentY + direction] != null && board[currentX + 1, currentY + direction].team != team){
                r.Add(new Vector2Int(currentX + 1, currentY + direction));
            }
        }
        
        //Negras
        if(currentX != 0){
            if(board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != team){
                r.Add(new Vector2Int(currentX - 1, currentY + direction));
            }
        }

        return r;
    }

    public override SpecialMove GetSpecialMoves(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves){
        int direction = (team == 0) ? 1 : -1;
        //para Promocion
        if((team == 0 && currentY == 6) || (team == 1 && currentY == 1)){
            return SpecialMove.Promotion;
        }
        //En Passant || Comer Al Paso
        if(moveList.Count > 0){
            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            //si la ultima pieza movida fue un peon
            if(board[lastMove[1].x, lastMove[1].y].type == ChessPieceType.Pawn){ 
                // si el ultimo movimiento fue un +2 en una direccion
                if(Mathf.Abs(lastMove[0].y - lastMove[1].y) == 2){
                    // si el movimiento lo realizo el otro equipo
                    if(board[lastMove[1].x, lastMove[1].y].team != team){
                        // si ambos peones estan en la misma linea
                        if(lastMove[1].y == currentY){
                            //Izquierda
                            if(lastMove[1].x == currentX - 1){
                                availableMoves.Add(new Vector2Int(currentX - 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }
                            //Derecha
                            if(lastMove[1].x == currentX + 1){
                                availableMoves.Add(new Vector2Int(currentX + 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }
                        }
                    }
                }
            }
        }

        return SpecialMove.None;
    }
}
