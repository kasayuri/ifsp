package com.example.servicos;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.servicos.api.InvertextoApi;
import com.example.servicos.model.Cotacao;

import java.util.Locale;
import java.util.Map;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class CurrencyActivity extends AppCompatActivity {
    private ProgressBar progressBar;
    private Button btConsultarCotacao;
    private Button btVoltar;
    private TextView tvResultadoCotacao;
    private EditText etValor;
    private Spinner spMoeda;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_currency);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        // Cria as variáveis ligadas a View (xml)
        etValor = findViewById(R.id.etValor);
        btConsultarCotacao = findViewById(R.id.btConsultaConversao);
        btVoltar = findViewById(R.id.btVoltar);
        progressBar = findViewById(R.id.progressBar2);
        tvResultadoCotacao = findViewById(R.id.tvResultadoCotacao);
        spMoeda = findViewById(R.id.spMoeda);
        spMoeda.setSelection(0);

        // Adiciona listener nos botoes
        btConsultarCotacao.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view){
                consultar(spMoeda.getSelectedItem().toString() + "_BRL");
            }
        });

        btVoltar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                finish();
            }
        });
    }

    private void consultar(String moeda){
        progressBar.setVisibility(View.VISIBLE);
        btConsultarCotacao.setEnabled(false);
        tvResultadoCotacao.setText("");
        if (etValor.getText().toString().isEmpty()){
            etValor.setText("1");
        }

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(Constantes.BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        InvertextoApi api = retrofit.create(InvertextoApi.class);

        Call<Map<String, Cotacao>> call = api.getCotacao(
                moeda,
                Constantes.TOKEN_CURRENCY
        );

        call.enqueue(new Callback<Map<String, Cotacao>>() {
            @Override
            public void onResponse(Call<Map<String, Cotacao>> call, Response<Map<String, Cotacao>> response) {
                progressBar.setVisibility(View.GONE);
                btConsultarCotacao.setEnabled(true);

                if (response.isSuccessful() && response.body() != null){
                    Map<String, Cotacao> cotacoes = response.body();

                    if (cotacoes.containsKey(moeda)) {
                        Cotacao detalhe = cotacoes.get(moeda);
                        StringBuilder resultado = new StringBuilder();

                        if (detalhe != null) {
                            resultado.append("  Cotação: R$ ").append(String.format(Locale.getDefault(),"%.4f", detalhe.getPrice())).append("\n");
                            resultado.append("  Valor calculado: R$ ").append(detalhe.getPrice() * Double.parseDouble(etValor.getText().toString())).append("\n");
                            resultado.append("  \nAtualizado em: ").append(detalhe.getTimestamp()).append("\n\n");

                        } else {
                            resultado.append("Detalhes da cotação não encontrados para " + moeda);
                        }

                        tvResultadoCotacao.setText(resultado.toString());
                    } else {
                        tvResultadoCotacao.setText("Nenhuma cotação encontrada para " + moeda);
                    }

                } else {
                    // retornar erro
                    Toast.makeText(
                            CurrencyActivity.this,
                            "Erro ao buscar cotações. Código: " + response.code(),
                            Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Map<String, Cotacao>> call, Throwable throwable) {
                progressBar.setVisibility(View.GONE);
                btConsultarCotacao.setEnabled(true);

                Toast.makeText(
                        CurrencyActivity.this,
                        "Verifique sua conexão com a internet.",
                        Toast.LENGTH_LONG).show();
            }
        });
    }

}