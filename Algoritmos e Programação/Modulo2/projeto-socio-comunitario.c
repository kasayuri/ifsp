#include <stdio.h>

int main() {
    int idade;

    printf("------------------------------------------------------------------\n");
    printf("    VALIDADOR SIMPLES DE IDADE PARA VACINA DA DENGUE   \n");
    printf("------------------------------------------------------------------\n");
    printf("Este programa ajuda a verificar se sua idade se enquadra nos criterios gerais para a vacina da dengue disponivel no SUS.\n");

    // 3. Coleta da Idade do Usuário
    printf("Por favor, digite sua idade em anos (apenas o numero): ");
    scanf("%d", &idade);

    printf("\n--- Resultado da Verificacao ---\n");

    if (idade < 6) {
        // Condição: Idade abaixo da faixa mínima
        printf("A vacina da dengue NAO E RECOMENDADA para criancas abaixo de 6 anos.\n");
    } else if (idade > 16) {
        // Condição: Idade acima da faixa máxima
        printf("A vacina da dengue NAO E RECOMENDADA para pessoas acima de 16 anos na campanha do SUS.\n");
        printf("Verifique se ha outras vacinas ou criterios em clinicas particulares, se for o caso.\n");
    } else {
        // Condição: Idade entre 6 e 16 anos (faixa de elegibilidade principal)
        printf("Sua idade (%d anos) se enquadra nos criterios para a vacina da dengue.\n", idade);
        printf("Procure a Unidade Basica de Saude (UBS) mais proxima para se vacinar e esclarecer suas duvidas.\n");
    }

    printf("\n------------------------------------------------------------------\n");
    printf("Lembre-se: Este programa e apenas um guia informativo. Consulte sempre um profissional de saude.\n");

    return 0; // Retorna 0 para indicar que o programa foi executado com sucesso
}
