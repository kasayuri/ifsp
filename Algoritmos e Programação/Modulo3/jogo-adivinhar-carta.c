#include <stdio.h>  
#include <stdlib.h> 
#include <time.h>   

int main() {
    // Inicializa o gerador de números aleatórios com base no tempo atual.
    // Isso garante que as "cartas" sejam diferentes a cada execução do jogo.
    srand(time(NULL));

    int pontuacao = 0;
    int erros = 0;
    int jogarNovamente = 1;

    // Arte realizada em https://www.asciiart.eu/text-to-ascii-art
    printf(".-----------------------------------------------------------------------------.\n");
    printf("|    _       _ _       _       _                                   _        _ |\n");
    printf("|   / \\   __| (_)_   _(_)_ __ | |__   ___    __ _    ___ __ _ _ __| |_ __ _| ||\n");
    printf("|  / _ \\ / _` | \\ \\ / / | '_ \\| '_ \\ / _ \\  / _` |  / __/ _` | '__| __/ _` | ||\n");
    printf("| / ___ \\ (_| | |\\ V /| | | | | | | |  __/ | (_| | | (_| (_| | |  | || (_| |_||\n");
    printf("|/_/   \\_\\__,_|_| \\_/ |_|_| |_|_| |_|\\___|  \\__,_|  \\___\\__,_|_|   \\__\\__,_(_)|\n");
    printf("'-----------------------------------------------------------------------------'\n\n");
    printf("- Você e o computador vão 'tirar' uma carta (valores de 1 a 13 - simulando As a Rei).\n");
    printf("- Seu objetivo e adivinhar se a carta do computador sera MAIOR, MENOR ou IGUAL a sua.\n\n");

    // Loop principal do jogo
    while (jogarNovamente == 1) {
        int suaCarta, cartaComputador;
        int chute; // 1: Maior, 2: Menor, 3: Igual
        suaCarta = (rand() % 13) + 1;

        printf("'-----------------------------------------------------------------------------'\n");
        printf("|Sua pontuacao atual: %d\n", pontuacao);
        printf("|Sua carta e: %d\n", suaCarta);
        printf("'-----------------------------------------------------------------------------'\n");

        // Loop para garantir que o usuário digite uma opção válida
        printf("A carta do computador sera:\n");
        printf("  1 - MAIOR que sua carta (%d)\n", suaCarta);
        printf("  2 - MENOR que sua carta (%d)\n", suaCarta);
        printf("  3 - IGUAL a sua carta (%d)\n", suaCarta);
        printf("Digite sua opcao (1, 2 ou 3): ");

        if (scanf("%d", &chute) != 1 || (chute < 1 || chute > 3)) {
            printf("Opcao invalida. Por favor, digite 1, 2 ou 3.\n");
            // Limpa o buffer de entrada para evitar problemas no próximo scanf
            while (getchar() != '\n');
            continue; // Pula para a próxima iteração do loop principal
        }
        
        // Limpa o buffer de entrada para o caso de o usuário digitar mais caracteres após o número
        while (getchar() != '\n');

        // Gera o valor da carta do computador
        cartaComputador = (rand() % 13) + 1;

        printf("\nO computador tirou a carta: %d\n", cartaComputador);

        int acertou = 0;

        // Verifica o chute do jogador com o resultado real
        if (chute == 1) { // Chute: MAIOR
            if (cartaComputador > suaCarta) {
                acertou = 1;
            }
        } else if (chute == 2) { // Chute: MENOR
            if (cartaComputador < suaCarta) {
                acertou = 1;
            }
        } else if (chute == 3) { // Chute: IGUAL
            if (cartaComputador == suaCarta) {
                acertou = 1;
            }
        }

        if (acertou) {
            printf("Parabens! Voce acertou!\n");
            pontuacao++;
        } else {
            printf("Que pena! Voce errou.\n");
            erros++;
        }

        // Pergunta se o jogador quer jogar novamente
        printf("\nDeseja jogar novamente? (1 para Sim, 0 para Nao): ");
        if (scanf("%d", &jogarNovamente) != 1 || (jogarNovamente != 0 && jogarNovamente != 1)) {
            printf("Opcao invalida. Assumindo 'Nao'.\n");
            jogarNovamente = 0; // Se a entrada for inválida, assume que o jogador não quer continuar
        }
        while (getchar() != '\n'); // Limpa o buffer
    }
    printf("'-----------------------------------------------------------------------------'\n");
    printf("|Sua pontuacao final foi: %d contra %d para o computador\n", pontuacao, erros);
    printf("'-----------------------------------------------------------------------------'");
    return 0;
}
