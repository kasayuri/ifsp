package com.example.servicos;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.example.servicos.api.InvertextoApi;
import com.example.servicos.model.Logradouro;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class CepActivity extends AppCompatActivity {
    private ProgressBar progressBar;
    private Button btBuscar;
    private TextView tvInfo;
    private EditText etCep;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        // Cria as variáveis ligadas a View (xml)
        etCep = findViewById(R.id.etCep);
        btBuscar = findViewById(R.id.btBuscar);
        progressBar = findViewById(R.id.progressBar);
        tvInfo = findViewById(R.id.tvInfo);

        // Adiciona um listener no botão
        btBuscar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view){
                String numeroCep = etCep.getText().toString();
                consultar(numeroCep);
            }
        });
    }

    private void consultar(String numeroCep){
        progressBar.setVisibility(View.VISIBLE);
        btBuscar.setEnabled(false);
        tvInfo.setText("");

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(Constantes.BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .build();

        InvertextoApi api = retrofit.create(InvertextoApi.class);

        Call<Logradouro> call = api.getLogradouro(
                numeroCep,
                Constantes.TOKEN_CEP
        );

        call.enqueue(new Callback<Logradouro>() {
            @Override
            public void onResponse(Call<Logradouro> call, Response<Logradouro> response) {
                progressBar.setVisibility(View.GONE);
                btBuscar.setEnabled(true);

                if (response.isSuccessful()){
                    Logradouro logradouro = response.body();
                    // Exibir no tvInfo as informações do logradouro
                    tvInfo.setText(logradouro.formatar());

                } else {
                    // retornar erro
                    Toast.makeText(
                            CepActivity.this,
                            "Erro ao buscar informações, verifique o CEP",
                            Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Logradouro> call, Throwable throwable) {
                progressBar.setVisibility(View.GONE);
                btBuscar.setEnabled(true);

                Toast.makeText(
                        CepActivity.this,
                        "Verifique sua conexão com a internet",
                        Toast.LENGTH_LONG).show();
            }
        });
    }
}